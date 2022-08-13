using EShop.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using EShop.Core.Interfaces;
using AutoMapper;
using EShop.Data.Interfaces;

namespace EShop.Web.Areas.Catalog.Pages.Product
{
    [Authorize(Permissions.Product.Delete)]
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
        public ProductVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dbEntity = _unitOfWork.ProductRepository.GetByID(id);
            if (dbEntity == null)
            {
                return NotFound();
            }
            Entity = _mapper.Map<ProductVM>(dbEntity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var dbEntity = _unitOfWork.ProductRepository.Get(x => x.Id == id, "ProductPromotions").FirstOrDefault();
            if (dbEntity == null)
            {
                return NotFound();
            }
            else if (dbEntity.ProductPromotions.Count > 0)
            {
                ModelState.Clear();
                ModelState.AddModelError("InUse", $"Product assigned to promotions can not be deleted.");
                Entity = _mapper.Map<ProductVM>(dbEntity);

                return Page();
            }

            _unitOfWork.ProductRepository.Delete(dbEntity);
            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }
    }
}
