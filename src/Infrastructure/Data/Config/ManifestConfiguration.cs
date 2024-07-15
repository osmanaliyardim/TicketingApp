using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class ManifestConfiguration : IEntityTypeConfiguration<Manifest>
{
    public void Configure(EntityTypeBuilder<Manifest> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
           .UseHiLo("manifest_hilo")
           .IsRequired();

        builder.Property(m => m.SeatMap)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasOne(m => m.Venue)
            .WithMany()
            .HasForeignKey(ci => ci.VenueId);
    }
}
