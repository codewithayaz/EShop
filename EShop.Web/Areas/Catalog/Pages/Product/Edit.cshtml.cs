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
    [Authorize(Permissions.Product.Update)]
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        [BindProperty]
        public ProductVM Entity { get; set; }

        public List<SelectListItem> Categories { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dbEntity = _unitOfWork.ProductRepository.GetByID(id);
            if (dbEntity == null)
            {
                return NotFound();
            }
            Categories = _unitOfWork.CategoryRepository.Get().Select(x =>
                                  new SelectListItem
                                  {
                                      Value = x.Id.ToString(),
                                      Text = x.Name
                                  }).ToList();

            Entity = _mapper.Map<ProductVM>(dbEntity);
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            ModelState.Remove("Entity.Category");
            ModelState.Remove("Entity.Promotion");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_unitOfWork.ProductRepository.Any(x => x.Name == Entity.Name && x.Id != Entity.Id))
            {
                ModelState.AddModelError("Name", $"{Entity.Name} already exists.");
                return Page();
            }

            var dbEntity = _unitOfWork.ProductRepository.GetByID(Entity.Id);
            dbEntity.CategoryId = Entity.CategoryId;
            dbEntity.Name = Entity.Name;
            dbEntity.Description = Entity.Description;
            dbEntity.Price = Entity.Price;
            dbEntity.ModifiedDate = DateTime.Now;
            _unitOfWork.ProductRepository.Update(dbEntity);
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }

    }
}
