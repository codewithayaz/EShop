using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using EShop.Web.Areas.Catalog.Pages.Product;

namespace EShop.Web.Areas.Catalog.Pages.Category
{
    public class CategoryVM
    {
        [HiddenInput]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Display(Name ="Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public List<ProductVM> Products { get; set; }
    }
}
