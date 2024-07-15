using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
           .UseHiLo("section_hilo")
           .IsRequired();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);
    
        builder.HasOne(s => s.Venue)
            .WithMany()
            .HasForeignKey(s => s.VenueId);
    }
}
