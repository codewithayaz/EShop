using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using EShop.Data.Interfaces;
using EShop.Data.Enums;

namespace EShop.Data.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TemporaryPassword { get; set; }
        [Required]
        public UserType UserType { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
