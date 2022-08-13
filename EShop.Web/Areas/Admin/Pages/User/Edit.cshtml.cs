using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EShop.Core.Constants;
using EShop.Data.Entities;
using EShop.Web.Areas.Admin.Pages.Role;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.User
{
    [Authorize(Permissions.User.Update)]
    public class EditModel : PageModel
    {
        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public EditModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
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

            var userRoles = _userManager.GetRolesAsync(entity).Result;
            Entity.Roles = _roleManager.Roles.AsNoTracking().Select(x => new RoleVM
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = userRoles.Contains(x.Name)
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Entity.Id);

            user.FirstName = Entity.FirstName;
            user.LastName = Entity.LastName;
            IdentityResult result = _userManager.UpdateAsync(user).Result;

            if (Entity.Roles != null)
            {
                try
                {
                    var newSelectedRoles = Entity.Roles.Where(x => x.IsSelected).Select(x=>x.Name).ToList();
                    var rolesToAdd = new List<string>();
                    var currentUserRoles = (List<string>) _userManager.GetRolesAsync(user).Result;

                    foreach (var newUserRole in newSelectedRoles)
                    {
                        if (!currentUserRoles.Contains(newUserRole))
                            rolesToAdd.Add(newUserRole);
                    }

                    if (rolesToAdd.Count > 0)
                    {
                        result = _userManager.AddToRolesAsync(user, rolesToAdd).Result;
                    }

                    var rolesToRemove = currentUserRoles.Where(role => !newSelectedRoles.Contains(role)).ToList();
                    if(user.UserType == Data.Enums.UserType.SystemAdmin)
                    {
                        // admin roles can not be deleted for super user
                        rolesToRemove.Remove(DefaultRoles.Admin);
                    }
                    if (rolesToRemove.Count > 0)
                    {
                        result = _userManager.RemoveFromRolesAsync(user, rolesToRemove).Result;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
