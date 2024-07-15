using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class PricesConfiguration : IEntityTypeConfiguration<Prices>
{
    public void Configure(EntityTypeBuilder<Prices> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
           .UseHiLo("prices_hilo")
           .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Offer)
            .WithMany()
            .HasForeignKey(p => p.OfferId);
    }
}
