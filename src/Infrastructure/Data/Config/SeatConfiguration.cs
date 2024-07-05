using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("seat_hilo")
           .IsRequired();

        builder.Property(t => t.SeatType)
            .IsRequired();

        builder.Property(t => t.IsAvailable)
            .IsRequired();

        builder.Property(t => t.Number)
            .IsRequired();

        builder.Property(t => t.Row)
            .HasMaxLength(25);

        builder.Property(t => t.Section)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(t => t.Manifest)
            .WithMany()
            .HasForeignKey(ci => ci.ManifestId);
    }
}
