using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Range(0, 1000000000000), DataType("decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<ProductPromotion> ProductPromotions { get; set; }
    }
}
