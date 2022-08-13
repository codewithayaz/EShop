using EShop.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using EShop.Core.Interfaces;
using AutoMapper;
using EShop.Data.Interfaces;

namespace EShop.Web.Areas.Catalog.Pages.Promotion
{
    [Authorize(Permissions.Promotion.Delete)]
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeleteModel(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var dbEntity = _unitOfWork.PromotionRepository.GetByID(id);
            if (dbEntity != null)
            {
                _unitOfWork.PromotionRepository.Delete(dbEntity);
                _unitOfWork.Save();
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
