using System.ComponentModel.DataAnnotations;
using EShop.Web.Areas.Catalog.Pages.Category;
using EShop.Web.Areas.Catalog.Pages.Product;

namespace EShop.Web.Areas.Catalog.Pages.Promotion
{
    public class PromotionVM
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Display(Name ="Discount Percentage"), Range(1, 100)]
        public int DiscountPercent { get; set; }
        [Display(Name = "Start Date"), DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date"), DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public List<CategoryVM> Categories { get; set; }
    }
}