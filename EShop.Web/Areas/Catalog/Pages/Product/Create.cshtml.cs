using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EShop.Core.Interfaces;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Catalog.Pages.Product
{
    [Authorize(Permissions.Product.Create)]
    public class CreateModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateModel(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<SelectListItem> Categories { get; set; }
        public void OnGet()
        {
            Categories = _unitOfWork.CategoryRepository.Get().Select(x =>
                                  new SelectListItem
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Name
                                  }).ToList();
            //return Page();
        }

        [BindProperty]
        public ProductVM Entity { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ModelState.Remove("Entity.Category");

                if (!ModelState.IsValid)
                {
                    Categories = _unitOfWork.CategoryRepository.Get().Select(x =>
                                 new SelectListItem
                                 {
                                     Value = x.Id.ToString(),
                                     Text = x.Name
                                 }).ToList();
                    return Page();
                }
                if(_unitOfWork.ProductRepository.Any(x=>x.Name == Entity.Name))
                {
                    ModelState.AddModelError("Name", $"{Entity.Name} already exists.");
                    return Page();
                }
                var dbEntity = _mapper.Map<Data.Entities.Product>(Entity);
                dbEntity.CreatedDate = DateTime.Now;
                dbEntity.ModifiedDate = null;
                _unitOfWork.ProductRepository.Insert(dbEntity);
                _unitOfWork.Save();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("InternalServerError", ex.Message);
                return Page();
            }
        }
    }
}
