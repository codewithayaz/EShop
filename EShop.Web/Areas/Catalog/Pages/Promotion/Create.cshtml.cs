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
    [Authorize(Permissions.Promotion.Create)]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PromotionVM Entity { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ModelState.Remove("Entity.Categories");
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                else if (_unitOfWork.PromotionRepository.Any(x => x.Name == Entity.Name))
                {
                    ModelState.AddModelError("Name", $"{Entity.Name} already exists.");
                    return Page();
                }
                var dbEntity = _mapper.Map<Data.Entities.Promotion>(Entity);
                dbEntity.CreatedDate = DateTime.Now;
                dbEntity.ModifiedDate = null;
                _unitOfWork.PromotionRepository.Insert(dbEntity);
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
