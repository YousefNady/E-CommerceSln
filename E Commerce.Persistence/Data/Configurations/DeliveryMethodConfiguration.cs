using E_Commerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Persistence.Data.Configurations
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(X => X.Price)
                   .HasPrecision(18, 2);

            builder.Property(X => X.ShortName).HasMaxLength(50);
            builder.Property(X => X.DeliveryTime).HasMaxLength(50);
            builder.Property(X => X.Description).HasMaxLength(100);
        }
    }
}
