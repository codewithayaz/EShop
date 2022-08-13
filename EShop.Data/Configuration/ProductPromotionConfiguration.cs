using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EShop.Data.Entities;

namespace EShop.Data.Configuration
{
    public class ProductPromotionConfiguration : IEntityTypeConfiguration<ProductPromotion>
    {
        public void Configure(EntityTypeBuilder<ProductPromotion> builder)
        {
            builder
                .HasKey(c => new { c.ProductId, c.PromotionId });

            builder
                .HasOne(c => c.Product)
                .WithMany(e => e.ProductPromotions)
                .HasForeignKey(x => x.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.Promotion)
                .WithMany(e => e.ProductPromotions)
                .HasForeignKey(x => x.PromotionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
