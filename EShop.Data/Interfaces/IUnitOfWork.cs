using EShop.Data.Entities;

namespace EShop.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<UserAudit> UserAuditRepository { get; }
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Promotion> PromotionRepository { get; }
        IGenericRepository<ProductPromotion> ProductPromotionRepository { get; }
        IGenericRepository<CartItem> CartItemRepository { get; }
        ApplicationDbContext ApplicationDbContext { get; }
        void Dispose();
        void Save();
    }
}
