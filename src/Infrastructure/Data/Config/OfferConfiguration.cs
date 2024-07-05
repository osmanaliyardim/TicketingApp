using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("offer_hilo")
           .IsRequired();

        builder.Property(t => t.StartDate)
            .IsRequired();

        builder.Property(t => t.EndDate)
            .IsRequired();

        builder.Property(t => t.Conditions)
            .IsRequired()
            .HasMaxLength(1250);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasOne(t => t.Event)
            .WithMany()
            .HasForeignKey(ci => ci.EventId);
    }
}
