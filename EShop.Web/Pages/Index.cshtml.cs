using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using EShop.Data.Interfaces;
using EShop.Web.Areas.Catalog.Pages.Category;
using EShop.Web.Areas.Catalog.Pages.Promotion;

namespace EShop.Web.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IndexModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<CategoryVM> Categories { get; set; }
        [BindProperty]
        public List<int> SelectedCategories { get; set; }
        [BindProperty]
        public int SortBy { get; set; }
        [BindProperty]
        public string OrderBy { get; set; }
        [BindProperty]
        public string Search { get; set; }
        public void OnGet()
        {
            var dbCategories = _unitOfWork.CategoryRepository.Get(includeProperties: "Products,Products.ProductPromotions.Promotion").Where(x => x.Products.Count > 0).ToList();
            Categories = _mapper.Map<List<CategoryVM>>(dbCategories);
            var now = DateTime.Now.Date;
            foreach (var category in Categories)
            {
                category.Products = category.Products.OrderBy(x => x.Price).ToList();
                foreach (var product in category.Products)
                {
                    var dbPromotion = dbCategories.First(x => x.Id == category.Id).Products.First(x => x.Id == product.Id)
                        .ProductPromotions.FirstOrDefault(x => x.Promotion.StartDate.Date <= now && x.Promotion.EndDate >= now)?.Promotion;
                    product.Promotion = _mapper.Map<PromotionVM>(dbPromotion);
                }
            }
            HttpContext.Session.SetString("Categories", JsonConvert.SerializeObject(Categories));
            
            ViewData["CategoryList"] = new SelectList(Categories, "Id", "Name");

            Categories = Categories.Where(x => x.Products.Count > 0).ToList();
        }

        public void OnPostUpdateFilter()
        {
            var categoriesString = HttpContext.Session.GetString("Categories");
          
            Categories = JsonConvert.DeserializeObject<List<CategoryVM>>(categoriesString);
            ViewData["CategoryList"] = new SelectList(Categories, "Id", "Name");

            if (SelectedCategories.Count > 0)
            {
                Categories = Categories.Where(x => SelectedCategories.Contains(x.Id)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(Search))
            {
                string search = Search.ToLower();
                foreach (var category in Categories)
                {
                    category.Products = category.Products.Where(x => x.Name.ToLower().Contains(search) ||
                    x.CreatedDate.ToString().Contains(search) || x.Price.ToString().Contains(search)).ToList();
                }
            }

            foreach (var category in Categories)
            {
                if (SortBy == 1)
                {
                    switch (OrderBy)
                    {
                        case "Name":
                            category.Products = category.Products.OrderBy(x => x.Name).ToList();
                            break;
                        case "Price":
                            category.Products = category.Products.OrderBy(x => x.Price).ToList();
                            break;
                        case "Date":
                            category.Products = category.Products.OrderBy(x => x.CreatedDate).ToList();
                            break;
                    }
                }
                else
                {
                    switch (OrderBy)
                    {
                        case "Name":
                            category.Products = category.Products.OrderByDescending(x => x.Name).ToList();
                            break;
                        case "Price":
                            category.Products = category.Products.OrderByDescending(x => x.Price).ToList();
                            break;
                        case "Date":
                            category.Products = category.Products.OrderByDescending(x => x.CreatedDate).ToList();
                            break;
                    }
                }
            }

            Categories = Categories.Where(x => x.Products.Count > 0).ToList();
        }
    }
}