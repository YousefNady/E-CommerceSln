using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(X => X.Price).HasPrecision(8, 2);

            builder.OwnsOne(X => X.Product, OEntity =>
            {
                OEntity.Property(X => X.ProductName).HasMaxLength(100);
                OEntity.Property(X => X.PictureUrl).HasMaxLength(200);
            });
        }
    }
}
