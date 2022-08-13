using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Constants
{
    public static class DefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string Customer = nameof(Customer);
        public static IReadOnlyList<string> Get { get; } = new ReadOnlyCollection<string>(new[]
        {
            Admin,
            Customer
        });

        public static bool IsDefault(string roleName) => Get.Any(r => r == roleName);

    }
}
