using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Core.Constants;
using EShop.Data.Interfaces;
using EShop.Web.Areas.Catalog.Pages.Category;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Catalog.Pages.Promotion
{
    [Authorize(Permissions.Promotion.Manage)]
    public class ManageModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ManageModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [BindProperty]
        public PromotionVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dbEntity = _unitOfWork.PromotionRepository.Get(x => x.Id == id, "ProductPromotions").FirstOrDefault();
            if (dbEntity == null)
            {
                return NotFound();
            }

            var overlappingProducts = _unitOfWork.PromotionRepository.Get(x => x.Id != dbEntity.Id &&
                    x.StartDate <= dbEntity.StartDate && x.EndDate >= dbEntity.EndDate, "ProductPromotions")
                    .SelectMany(x=>x.ProductPromotions).Select(x=>x.Product.Id);


            Entity = _mapper.Map<PromotionVM>(dbEntity);
            var productPromotions = _unitOfWork.ProductPromotionRepository.Get(x => x.PromotionId == id);
            //var categories = _unitOfWork.ApplicationDbContext.Categories.Include(x => x.Products);
            var categories = _unitOfWork.CategoryRepository.Get(includeProperties: "Products").Where(x => x.Products.Count > 0).ToList();
            Entity.Categories = _mapper.Map<List<CategoryVM>>(categories);
            foreach (var category in Entity.Categories)
            {
                category.Products = category.Products.Where(x => !overlappingProducts.Contains(x.Id)).ToList();
                foreach (var product in category.Products)
                {
                    product.IsSelected = dbEntity.ProductPromotions.Any(x => x.ProductId == product.Id);
                }
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var selectedProducts = Entity.Categories.Where(x=>x.Products!=null)
                .SelectMany(x => x.Products).Where(x => x.IsSelected).ToList();
            

            //var promotion = _unitOfWork.PromotionRepository.GetByID(Entity.Id);
            var promotion = _unitOfWork.PromotionRepository.Get(x => x.Id == Entity.Id, "ProductPromotions").First();



            var duplicatePromotion = _unitOfWork.PromotionRepository.Get(x => x.Id != promotion.Id &&
                    x.StartDate <= promotion.StartDate && x.EndDate >= promotion.EndDate &&
                    x.ProductPromotions.Any(x => selectedProducts.Select(i => i.Id).Contains(x.ProductId)), "ProductPromotions").FirstOrDefault();
            
            if (duplicatePromotion!=null)
            {
                var duplicatePromotionProducts = duplicatePromotion.ProductPromotions.Where(x => selectedProducts.Select(i => i.Id).Contains(x.ProductId)).Select(x=>x.Product).ToList();
                if (duplicatePromotionProducts.Count() > 0)
                {
                    var productNames = duplicatePromotionProducts.Select(x => x.Name).ToList();
                    ModelState.AddModelError("Name", $"{string.Join(", ", productNames)} is/are already associated with promotion {duplicatePromotion.Name}.");
                    return Page();
                }

            }

            if (promotion.ProductPromotions == null)
            {
                promotion.ProductPromotions = new List<Data.Entities.ProductPromotion>();
            }
            foreach (var item in promotion.ProductPromotions)
            {
                _unitOfWork.ProductPromotionRepository.Delete(item);
            }

            foreach (var item in selectedProducts)
            {
                _unitOfWork.ProductPromotionRepository.Insert(new Data.Entities.ProductPromotion
                {
                    ProductId = item.Id,
                    PromotionId = promotion.Id
                });
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }

    }
}
