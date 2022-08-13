using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Core;
using EShop.Core.Constants;
using EShop.Data;
using EShop.Data.Entities;
using EShop.Data.Interfaces;
using EShop.Web.Authorization;

namespace EShop.Web
{
    public static class DataSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            try
            {
                //IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

                using (IServiceScope scope = serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    //var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    //context.Database.Migrate();

                    UserManager<ApplicationUser> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    #region Seed Roles
                    await SeedRoleAsync(roleManager, DefaultRoles.Admin, ApplicationPermissions.GetAllPermissionValues());
                    await SeedRoleAsync(roleManager, DefaultRoles.Customer, new string[] { Permissions.Promotion.Read, Permissions.Product.Read, Permissions.Category.Read });

                    #endregion

                    // User Info
                    string firstName = "Super";
                    string lastName = "Admin";
                    string email = "superadmin@admin.com";
                    string password = "Qwaszx123$";

                    if (await _userManager.FindByNameAsync(email) == null)
                    {
                        // Create user account if it doesn't exist
                        ApplicationUser user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = firstName,
                            LastName = lastName,
                            TemporaryPassword = password,
                            UserType = Data.Enums.UserType.SystemAdmin
                        };

                        IdentityResult result = await _userManager.CreateAsync(user, password);

                        // Assign role to the user
                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, DefaultRoles.Admin);
                            await _userManager.AddToRoleAsync(user, DefaultRoles.Customer);
                        }
                    }


                    await SeedCatalogAsync(unitOfWork);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private static async Task SeedRoleAsync(RoleManager<IdentityRole> _roleManager, string roleName, string[] claims)
        {
            if ((await _roleManager.FindByNameAsync(roleName)) == null)
            {
                if (claims == null)
                    claims = new string[] { };

                string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
                if (invalidClaims.Any())
                    throw new Exception("The following claim types are invalid: " + string.Join(", ", invalidClaims));

                IdentityRole applicationRole = new IdentityRole(roleName);

                var result = await _roleManager.CreateAsync(applicationRole);

                IdentityRole role = await _roleManager.FindByNameAsync(applicationRole.Name);

                foreach (string claim in claims.Distinct())
                {
                    result = await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                    if (!result.Succeeded)
                    {
                        await _roleManager.DeleteAsync(role);
                    }
                }
            }
        }

        private static async Task SeedCatalogAsync(IUnitOfWork unitOfWork)
        {
            if (!unitOfWork.CategoryRepository.Get().Any())
            {
                Random rnd = new Random();
                string[] categories = { "Mobiles", "Laptops", "Electronics", "Books", "Medicines", "Clothes", "Shoes", "Childrens" };
                foreach (string category in categories)
                {
                    Category dbCategory = new Category
                    {
                        Name = category,
                        CreatedDate = DateTime.Now,
                        ModifiedDate=null,
                        Products = new List<Product>()
                    };
                    for (int i = 1; i <= 20; i++)
                    {
                        dbCategory.Products.Add(new Product
                        {
                            Name = category + " " + i,
                            Price = new decimal(rnd.Next(10000, 70000)),
                            CreatedDate = DateTime.Now,
                            ModifiedDate = null
                        });
                    }
                    unitOfWork.CategoryRepository.Insert(dbCategory);
                    unitOfWork.Save();
                }
            }

        }
    }
}
