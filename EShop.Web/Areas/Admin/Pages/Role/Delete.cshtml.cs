using EShop.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EShop.Web.Areas.Admin.Pages.Role
{
    [Authorize(Permissions.Role.Delete)]
    public class DeleteModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty]
        public RoleVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }
            Entity = new RoleVM(role);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (Entity.IsDefault)
            {
                ModelState.AddModelError("Name", $"System roles can not be deleted.");
                return Page();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }

            return RedirectToPage("./Index");
        }
    }
}
