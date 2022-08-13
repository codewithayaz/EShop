using System.ComponentModel.DataAnnotations;
namespace EShop.Data.Enums
{
    public enum UserType
    {
        [Display(Name = "Master Admin")]
        SystemAdmin = 1,
        [Display(Name = "Admin")]
        Admin = 2,
        [Display(Name = "Customer")]
        Customer = 3
    }
}
