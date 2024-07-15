using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(oi => oi.ItemOrdered, oi =>
        {
            oi.WithOwner();

            oi.Property(eo => eo.EventName)
                .IsRequired()
                .HasMaxLength(75);
        });

        builder.Property(oi => oi.UnitPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");
    }
}
