using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using EShop.Web.Areas.Catalog.Pages.Category;
using EShop.Web.Areas.Catalog.Pages.Promotion;

namespace EShop.Web.Areas.Catalog.Pages.Product
{
    public class ProductVM
    {
        [HiddenInput]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Range(0.0, 1000000000000)]
        public decimal Price { get; set; }

        public decimal Discount
        {
            get
            {
                decimal discount = 0;
                if (Promotion != null)
                {
                    discount = Promotion.DiscountPercent / 100m * Price;
                }
                return Math.Round(discount, MidpointRounding.AwayFromZero);
            }
        }
        public decimal DiscountedPrice
        {
            get
            {
                decimal price = Price-Discount;
                
                return Math.Round(price, MidpointRounding.AwayFromZero);
            }
        }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public bool IsSelected { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [JsonIgnore]
        public CategoryVM Category { get; set; }
        public PromotionVM Promotion { get; set; }
    }
}
