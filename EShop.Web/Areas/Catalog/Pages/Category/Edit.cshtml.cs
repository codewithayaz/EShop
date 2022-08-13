using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShop.Core.Interfaces;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Catalog.Pages.Category
{
    [Authorize(Permissions.Category.Update)]
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
        public CategoryVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dbEntity = _unitOfWork.CategoryRepository.GetByID(id);
            if (dbEntity == null)
            {
                return NotFound();
            }
            Entity = _mapper.Map<CategoryVM>(dbEntity);
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Entity.Products");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_unitOfWork.CategoryRepository.Any(x => x.Name == Entity.Name && x.Id != Entity.Id))
            {
                ModelState.AddModelError("Name", $"{Entity.Name} already exists.");
                return Page();
            }
            var dbEntity = _unitOfWork.CategoryRepository.GetByID(Entity.Id);
            dbEntity.Name = Entity.Name;
            dbEntity.Description = Entity.Description;
            dbEntity.ModifiedDate = DateTime.Now;
            _unitOfWork.CategoryRepository.Update(dbEntity);
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }

    }
}
