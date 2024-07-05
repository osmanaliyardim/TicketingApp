using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("event_hilo")
           .IsRequired();

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(t => t.Time)
            .IsRequired();

        builder.HasOne(t => t.Venue)
            .WithMany()
            .HasForeignKey(ci => ci.VenueId);
    }
}
