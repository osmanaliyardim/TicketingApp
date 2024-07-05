using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("venue_hilo")
           .IsRequired();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(t => t.Capacity)
            .IsRequired();

        builder.Property(t => t.Location)
            .IsRequired()
            .HasMaxLength(1250);
    } 
}
