using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EShop.Data.Entities;

namespace EShop.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder
                .HasKey(c => new { c.ProductId, c.UserId });

            builder
                .HasOne(c => c.Product)
                .WithMany(e => e.CartItems)
                .HasForeignKey(x=>x.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.User)
                .WithMany(e => e.CartItems)
                .HasForeignKey(x=>x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
