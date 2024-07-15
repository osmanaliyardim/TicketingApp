using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
           .UseHiLo("offer_hilo")
           .IsRequired();

        builder.Property(o => o.StartDate)
            .IsRequired();

        builder.Property(o => o.EndDate)
            .IsRequired();

        builder.Property(o => o.Conditions)
            .IsRequired()
            .HasMaxLength(1250);

        builder.Property(o => o.Description)
            .IsRequired()
            .HasMaxLength(750);

        builder.HasOne(o => o.Event)
            .WithMany()
            .HasForeignKey(o => o.EventId);
    }
}
