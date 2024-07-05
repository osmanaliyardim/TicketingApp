using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class ManifestConfiguration : IEntityTypeConfiguration<Manifest>
{
    public void Configure(EntityTypeBuilder<Manifest> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("manifest_hilo")
           .IsRequired();

        builder.Property(t => t.SeatMap)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasOne(t => t.Venue)
            .WithMany()
            .HasForeignKey(ci => ci.VenueId);
    }
}
