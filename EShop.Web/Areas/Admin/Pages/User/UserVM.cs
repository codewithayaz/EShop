using System.ComponentModel.DataAnnotations;
using EShop.Core;
using EShop.Data.Entities;
using EShop.Data.Enums;
using EShop.Web.Areas.Admin.Pages.Role;

namespace EShop.Web.Areas.Admin.Pages.User
{
    public class UserVM
    {
        public UserVM()
        {

        }

        public UserVM(ApplicationUser user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
        }

        public string? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? TemporaryPassword { get; set; }
        [Required]
        public UserType UserType { get; set; }
        public List<RoleVM> Roles { get; set; }
        public ApplicationUser ToDbEntity()
        {
            return new ApplicationUser
            {
                UserName = this.Email,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                TemporaryPassword = Helper.RandomString()
            };
        }
    }

}
