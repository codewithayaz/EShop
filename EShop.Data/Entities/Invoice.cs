using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class Invoice : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }

    }
}
