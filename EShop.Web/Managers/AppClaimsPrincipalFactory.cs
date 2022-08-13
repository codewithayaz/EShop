using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using EShop.Data.Entities;

namespace EShop.Web.Managers
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;


            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                identity.AddClaims(new[] { new Claim(ClaimTypes.GivenName, user.FirstName) });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                identity.AddClaims(new[] { new Claim(ClaimTypes.Surname, user.LastName) });
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                identity.AddClaims(new[] { new Claim(ClaimTypes.Email, user.Email) });
            }


            //if (user.Email == "readonly@EShop.Web.com")
            //{
            //    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("ReadOnly", "true"));
            //}

            return principal;
        }
    }
}