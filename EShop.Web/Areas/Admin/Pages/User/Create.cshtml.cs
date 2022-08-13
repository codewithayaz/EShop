using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Core.Constants;
using EShop.Data.Entities;
using EShop.Web.Areas.Admin.Pages.Role;
using EShop.Web.Authorization;

namespace EShop.Web.Areas.Admin.Pages.User
{
    [Authorize(Permissions.User.Create)]
    public class CreateModel : PageModel
    {

        private readonly EShop.Data.ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public CreateModel(EShop.Data.ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        public IActionResult OnGet()
        {
            Entity = new UserVM();
            Entity.Roles = _roleManager.Roles.AsNoTracking().Select(x => new RoleVM
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                IsSelected = x.Name == DefaultRoles.Customer
            }).ToList();

            return Page();
        }

        [BindProperty]
        public UserVM Entity { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var user = Entity.ToDbEntity();
                user.UserType = Data.Enums.UserType.Customer;
                IdentityResult result = await _userManager.CreateAsync(user, user.TemporaryPassword);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(result.Errors.First().Code, result.Errors.First().Description);
                    return Page();
                }
                else
                {
                    var claimsResult = _userManager.AddClaimsAsync(user, new Claim[]{
                        new Claim(CustomClaimTypes.Name, Entity.Email),
                        new Claim(CustomClaimTypes.Email, Entity.Email),
                        new Claim(CustomClaimTypes.GivenName, Entity.FirstName),
                        new Claim(CustomClaimTypes.Surname, Entity.LastName),

                    }).Result;

                    if (Entity.Roles != null && Entity.Roles.Count > 0)
                    {
                        foreach (var role in Entity.Roles)
                        {
                            if (role.IsSelected)
                            {
                                await _userManager.AddToRoleAsync(user, role.Name);
                            }
                        }
                    }


                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("InternalServerError", ex.Message);
                return Page();
            }
        }
    }
}
