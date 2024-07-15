using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;

namespace TicketingApp.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(o => o.BuyerId)
            .IsRequired()
            .HasMaxLength(256);

        builder.OwnsOne(o => o.ShipToAddress, o =>
        {
            o.WithOwner();

            o.Property(o => o.ZipCode)
                .HasMaxLength(18)
                .IsRequired();

            o.Property(o => o.Street)
                .HasMaxLength(180)
                .IsRequired();

            o.Property(o => o.State)
                .HasMaxLength(60);

            o.Property(o => o.Country)
                .HasMaxLength(90)
                .IsRequired();

            o.Property(o => o.City)
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Navigation(o => o.ShipToAddress).IsRequired();
    }
}
