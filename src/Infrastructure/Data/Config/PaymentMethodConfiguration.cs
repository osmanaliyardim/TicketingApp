using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities.BuyerAggregate;

namespace TicketingApp.Infrastructure.Data.Config;

//public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
//{
//    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
//    {
//        builder.HasKey(s => s.Id);

//        builder.Property(s => s.Id)
//           .UseHiLo("paymentmethod_hilo")
//           .IsRequired();

//        builder.Property(s => s.Alias)
//            .HasMaxLength(128);

//        builder.Property(s => s.CardId)
//            .IsRequired()
//            .HasMaxLength(16);

//        builder.Property(s => s.Last4)
//            .IsRequired()
//            .HasMaxLength(4);
//    }
//}
