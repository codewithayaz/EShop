using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class InvoiceDetail : IEntity
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

        public Product Product { get; set; }
        public Invoice Invoice { get; set; }
        public Promotion? Promotion { get; set; }
    }
}
