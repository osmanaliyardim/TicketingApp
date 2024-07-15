using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.Emit;

namespace TicketingApp.Infrastructure.Data.Config;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
           .UseHiLo("seat_hilo")
           .IsRequired();

        builder.Property(s => s.SeatType)
            .IsRequired();

        builder.Property(s => s.IsAvailable)
            .IsRequired();

        builder.Property(s => s.Number)
            .IsRequired();

        builder.Property(s => s.Row)
            .HasMaxLength(25);

        builder.HasOne(s => s.Manifest)
            .WithMany()
            .HasForeignKey(s => s.ManifestId);

        builder.HasOne(s => s.Section)
            .WithMany()
            .HasForeignKey(s => s.SectionId);
    }
}
