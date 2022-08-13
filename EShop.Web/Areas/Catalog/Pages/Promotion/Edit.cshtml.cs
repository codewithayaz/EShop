using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShop.Core.Interfaces;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Catalog.Pages.Promotion
{
    [Authorize(Permissions.Promotion.Update)]
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
        public PromotionVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dbEntity = _unitOfWork.PromotionRepository.GetByID(id);
            if (dbEntity == null)
            {
                return NotFound();
            }
            Entity = _mapper.Map<PromotionVM>(dbEntity);
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Entity.Categories");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_unitOfWork.PromotionRepository.Any(x => x.Name == Entity.Name && x.Id != Entity.Id))
            {
                ModelState.AddModelError("Name", $"{Entity.Name} already exists.");
                return Page();
            }
            //var dbEntity = _mapper.Map<Data.Entities.Promotion>(Entity);
            var dbEntity = _unitOfWork.PromotionRepository.GetByID(Entity.Id);
            dbEntity.Name = Entity.Name;
            dbEntity.Description = Entity.Description;
            dbEntity.DiscountPercent = Entity.DiscountPercent;
            dbEntity.StartDate = Entity.StartDate;
            dbEntity.EndDate = Entity.EndDate;
            dbEntity.IsActive = Entity.IsActive;
            dbEntity.ModifiedDate = DateTime.Now;
            _unitOfWork.PromotionRepository.Update(dbEntity);
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }

    }
}
