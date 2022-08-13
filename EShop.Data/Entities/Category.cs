using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
