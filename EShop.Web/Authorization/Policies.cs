using Microsoft.AspNetCore.Authorization;
using EShop.Core.Constants;

namespace EShop.Web.Authorization
{
    public static class Policies
    {
        public const string IsSuperAdmin = nameof(IsSuperAdmin);
        public const string IsAdmin = nameof(IsAdmin);
        public const string IsCustomer = nameof(IsCustomer);

        public static AuthorizationPolicy IsSuperAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(nameof(IsSuperAdmin))
                .Build();
        }
        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(IsAdmin))
                .Build();
        }

        public static AuthorizationPolicy IsCustomerPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(DefaultRoles.Customer)
                .Build();
        }
    }
}
