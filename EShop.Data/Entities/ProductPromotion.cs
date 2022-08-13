using EShop.Data.Interfaces;

namespace EShop.Data.Entities
{
    public class ProductPromotion : IEntity
    {
        public int PromotionId { get; set; }
        public int ProductId { get; set; }

        public Promotion Promotion { get; set; }
        public Product Product { get; set; }
    }
}
