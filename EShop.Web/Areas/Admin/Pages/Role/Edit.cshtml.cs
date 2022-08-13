using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.Role
{
    [Authorize(Permissions.Role.Update)]
    public class EditModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EditModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Entity.IsDefault)
            {
                ModelState.AddModelError("Name", $"System roles can not be modified.");
                return Page();
            }
            var role = await _roleManager.FindByIdAsync(Entity.Id);

            role.Name = Entity.Name;
            var updateResult = _roleManager.UpdateAsync(role);

            return RedirectToPage("./Index");
        }

    }
}
