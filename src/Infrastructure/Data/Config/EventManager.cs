using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data.Config;

public class EventManagerConfiguration : IEntityTypeConfiguration<EventManager>
{
    public void Configure(EntityTypeBuilder<EventManager> builder)
    {
        builder.HasKey(em => em.Id);

        builder.Property(em => em.Id)
           .UseHiLo("eventmanager_hilo")
           .IsRequired();

        builder.Property(em => em.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(em => em.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(em => em.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(em => em.PasswordHash)
            .IsRequired();
    }
}
