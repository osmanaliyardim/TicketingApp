using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
           .UseHiLo("customer_hilo")
           .IsRequired();

        builder.Property(t => t.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.PasswordHash)
            .IsRequired();

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100);
    }
}
