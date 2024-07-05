using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
               .UseHiLo("ticket_hilo")
               .IsRequired();

            builder.Property(t => t.PurchaseDate)
                .IsRequired();

            builder.Property(t => t.IsPrinted)
                .IsRequired(); 

            builder.HasOne(t => t.Event)
                .WithMany()
                .HasForeignKey(ci => ci.EventId);

            builder.HasOne(t => t.Customer)
                .WithMany()
                .HasForeignKey(ci => ci.CustomerId);

            builder.HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(ci => ci.SeatId);

            builder.HasOne(t => t.Prices)
                .WithMany()
                .HasForeignKey(ci => ci.PricesId);

            builder.HasOne(t => t.Venue)
                .WithMany()
                .HasForeignKey(ci => ci.VenueId);
        }
    }
}
