using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShop.Data.Entities;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.User
{
    [Authorize(Permissions.User.Delete)]
    [Authorize(Policy ="SuperAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DeleteModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [BindProperty]
        public UserVM Entity { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _userManager.FindByIdAsync(id);

            if (entity == null)
            {
                return NotFound();
            }
            Entity = new UserVM(entity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.UserType == EShop.Data.Enums.UserType.SystemAdmin)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", $"User {user.Email} cannot be deleted.");
                    Entity = new UserVM(user);

                    return Page();
                }

                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            return RedirectToPage("./Index");
        }
    }
}
