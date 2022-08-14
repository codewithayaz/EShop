using EShop.Data.Entities;
using EShop.Data.Interfaces;
using EShop.Web.Areas.Admin.Pages.User;
using EShop.Web.Areas.Catalog.Pages.Product;
using EShop.Web.Areas.Catalog.Pages.Promotion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Web.Areas.Customer.Pages.Invoice
{
    public class InvoiceVM
    {
        [HiddenInput]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public UserVM User { get; set; }
        public ICollection<InvoiceDetailVM> InvoiceDetails { get; set; }

        public decimal Total
        {
            get
            {
                return Math.Round(InvoiceDetails.Sum(x=>x.Product.DiscountedPrice), MidpointRounding.AwayFromZero);
            }
        }
    }

    public class InvoiceDetailVM
    {
        public int Id { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PromotionDiscount { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public int ProductId { get; set; }
        public int InvoiceId { get; set; }
        public int? PromotionId { get; set; }

        public ProductVM Product { get; set; }
        public InvoiceVM Invoice { get; set; }
        public PromotionVM? Promotion { get; set; }
    }
}
