using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using EShop.Core.Constants;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.Role
{
    [Authorize(Permissions.Role.Update)]
    public class ManagePermissionModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagePermissionModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
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

            var claims = await _roleManager.GetClaimsAsync(role);

            List<ApplicationPermission> allPermissions = ApplicationPermissions.AllPermissions.ToList();
            if (Entity.Name == DefaultRoles.Customer)
            {
                allPermissions = allPermissions.Where(x => x.Value.EndsWith(".Read")).ToList();
            }

            Entity.PermissionGroups = allPermissions.GroupBy(x=>x.GroupName).Select(x => new PermissionGroup
            {
                Name = x.Key,
                Permissions=x.Select(p=>new ApplicationPermission
                {
                    Name = p.Name,
                    Value = p.Value,
                    Description = p.Description,
                    GroupName = p.GroupName,
                    IsSelected = claims.Any(c => c.Type == CustomClaimTypes.Permission && c.Value == p.Value)
                }).ToList()
            }).ToList();
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var role = await _roleManager.FindByIdAsync(Entity.Id);

            var selectedPermissions = Entity.PermissionGroups.SelectMany(x => x.Permissions).Where(x => x.IsSelected);

            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims.Where(x => x.Type == CustomClaimTypes.Permission).Select(x => x.Value).ToList();

            foreach (var permission in permissions)
            {
                await _roleManager.RemoveClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }

            foreach (var claim in selectedPermissions)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, claim.Value));

                //if (!result.Succeeded)
                //    await _roleManager.DeleteAsync(role);
            }


            return RedirectToPage("./Index");
        }

    }
}
