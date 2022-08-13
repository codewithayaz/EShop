using System.ComponentModel.DataAnnotations;
using EShop.Web.Areas.Catalog.Pages.Product;

namespace EShop.Web.Models
{
    public class CartItemVM
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        [Range(1, 10)]
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public ProductVM Product { get; set; }
    }
}
