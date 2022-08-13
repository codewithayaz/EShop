using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EShop.Data.Entities;

namespace EShop.Data.Configuration
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasMany(c => c.Products)
                .WithOne(e => e.Category)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
