using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class CartItem : IEntity
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        [Range(1,10)]
        public int Quantity { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}
