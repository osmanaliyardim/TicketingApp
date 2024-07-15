using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
           .UseHiLo("venue_hilo")
           .IsRequired();

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(v => v.Capacity)
            .IsRequired();

        builder.Property(v => v.Location)
            .IsRequired()
            .HasMaxLength(1250);
    } 
}
