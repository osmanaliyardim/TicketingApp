using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data;

public class TicketingContext : DbContext
{
    public TicketingContext(DbContextOptions<TicketingContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventManager> EventManagers { get; set; }
    public DbSet<Manifest> Manifests { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Prices> Prices { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
