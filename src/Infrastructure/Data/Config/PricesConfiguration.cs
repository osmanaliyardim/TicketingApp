using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class PricesConfiguration : IEntityTypeConfiguration<Prices>
{
    public void Configure(EntityTypeBuilder<Prices> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("prices_hilo")
           .IsRequired();

        builder.Property(t => t.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(t => t.Offer)
            .WithMany()
            .HasForeignKey(ci => ci.OfferId);
    }
}
