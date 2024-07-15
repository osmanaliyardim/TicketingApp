using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Entities.BuyerAggregate;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;

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
    public DbSet<ApplicationCore.Entities.Ticket> Tickets { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    //public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
        }

        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
