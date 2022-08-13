using EShop.Web.Authorization;
using EShop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EShop.Core.Constants;

namespace EShop.Web.Areas.Admin.Pages.Role
{
    public class RoleVM
    {
        public RoleVM()
        {

        }

        public RoleVM(IdentityRole role)
        {
            this.Id = role.Id;
            this.Name = role.Name;
        }

        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDefault
        {
            get
            {
                return DefaultRoles.Get.Contains(Name);
            }
        }

        public List<PermissionGroup>? PermissionGroups { get; set; }
    }

    public class PermissionGroup
    {
        public string Name { get; set; }
        public List<ApplicationPermission> Permissions { get; set; }
    };
}
