using EShop.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using EShop.Core.Interfaces;
using AutoMapper;
using EShop.Data.Interfaces;

namespace EShop.Web.Areas.Catalog.Pages.Category
{
    [Authorize(Permissions.Category.Delete)]
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var dbEntity = _unitOfWork.CategoryRepository.Get(x=>x.Id==id, "Products").First();
            if (dbEntity == null)
            {
                return NotFound();
            }
            else if (dbEntity.Products.Count > 0)
            {
                ModelState.Clear();
                ModelState.AddModelError("InUse", $"Category assigned to products can not be deleted.");
                Entity = _mapper.Map<CategoryVM>(dbEntity);

                return Page();
            }

            _unitOfWork.CategoryRepository.Delete(dbEntity);
            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }
    }
}
