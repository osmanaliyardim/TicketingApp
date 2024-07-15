using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.ApplicationCore.Entities.BuyerAggregate;

namespace TicketingApp.Infrastructure.Data.Config
{
    public class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
               .UseHiLo("buyer_hilo")
               .IsRequired();

            builder.Property(s => s.IdentityGuid)
                .IsRequired();
        }
    }
}
