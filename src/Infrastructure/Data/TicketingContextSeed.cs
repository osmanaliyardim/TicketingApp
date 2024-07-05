using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.Infrastructure.Data;

public class TicketingContextSeed
{
    public static async Task SeedAsync(TicketingContext ticketingContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (ticketingContext.Database.IsSqlServer())
            {
                ticketingContext.Database.Migrate();
            }

            if (!await ticketingContext.Venues.AnyAsync())
            {
                await ticketingContext.Venues.AddRangeAsync(GetPreconfiguredVenues());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Events.AnyAsync())
            {
                await ticketingContext.Events.AddRangeAsync(GetPreconfiguredEvents());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Manifests.AnyAsync())
            {
                await ticketingContext.Manifests.AddRangeAsync(GetPreconfiguredManifests());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Seats.AnyAsync())
            {
                await ticketingContext.Seats.AddRangeAsync(GetPreconfiguredSeats());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Offers.AnyAsync())
            {
                await ticketingContext.Offers.AddRangeAsync(GetPreconfiguredOffers());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Prices.AnyAsync())
            {
                await ticketingContext.Prices.AddRangeAsync(GetPreconfiguredPrices());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Customers.AnyAsync())
            {
                await ticketingContext.Customers.AddRangeAsync(GetPreconfiguredCustomers());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.EventManagers.AnyAsync())
            {
                await ticketingContext.EventManagers.AddRangeAsync(GetPreconfiguredEventManagers());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Tickets.AnyAsync())
            {
                await ticketingContext.Tickets.AddRangeAsync(GetPreconfiguredTickets());
                await ticketingContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;

            logger.LogError(ex.Message);
            await SeedAsync(ticketingContext, logger, retryForAvailability);
            throw;
        }
    }

    private static IEnumerable<Venue> GetPreconfiguredVenues()
    {
        return new List<Venue>
            {
                new Venue { Name = "Stadium A", Location = "City A", Capacity = 50000 },
                new Venue { Name = "Arena B", Location = "City B", Capacity = 20000 },
            };
    }

    private static IEnumerable<Event> GetPreconfiguredEvents()
    {
        return new List<Event>
            {
                new Event { Name = "Concert X", VenueId = 1, Date = new DateTime(2024, 8, 1), Time = new TimeSpan(19, 0, 0), Description = "Concert X by Band Y" },
                new Event { Name = "Sports Game Z", VenueId = 2, Date = new DateTime(2024, 8, 15), Time = new TimeSpan(18, 0, 0), Description = "Game Z Championship" },
            };
    }

    private static IEnumerable<Manifest> GetPreconfiguredManifests()
    {
        return new List<Manifest>
            {
                new Manifest { VenueId = 1, SeatMap = "Serialized Seat Map Data for Stadium A" },
                new Manifest { VenueId = 2, SeatMap = "Serialized Seat Map Data for Arena B" },
            };
    }

    private static IEnumerable<Seat> GetPreconfiguredSeats()
    {
        return new List<Seat>
            {
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "A", Number = 1, Section = "1", IsAvailable = true },
                new Seat { ManifestId = 1, SeatType = SeatTypes.GeneralAdmission, Section = "GA", IsAvailable = true },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "B", Number = 2, Section = "2", IsAvailable = true },
                new Seat { ManifestId = 2, SeatType = SeatTypes.GeneralAdmission, Section = "GA", IsAvailable = true },
            };
    }

    private static IEnumerable<Offer> GetPreconfiguredOffers()
    {
        return new List<Offer>
            {
                new Offer { EventId = 1, Description = "Early Bird Offer", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Conditions = "{ 'eligibility': { 'min_tickets': 1, 'max_tickets': 4 }, 'discount': 10 }" },
                new Offer { EventId = 2, Description = "Group Discount", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1), Conditions = "{ 'eligibility': { 'min_tickets': 5, 'max_tickets': 10 }, 'discount': 15 }" },
        };
    }

    private static IEnumerable<Prices> GetPreconfiguredPrices()
    {
        return new List<Prices>
        {
                new Prices { OfferId = 1, Category = Categories.Adult, Price = 50.00m },
                new Prices { OfferId = 1, Category = Categories.Child, Price = 30.00m },
                new Prices { OfferId = 2, Category = Categories.Adult, Price = 70.00m },
                new Prices { OfferId = 2, Category = Categories.VIP, Price = 150.00m },
            };
    }

    private static IEnumerable<Customer> GetPreconfiguredCustomers()
    {
        return new List<Customer>
            {
                new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PasswordHash = "hashedpassword" },
                new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PasswordHash = "hashedpassword" },
            };
    }

    private static IEnumerable<EventManager> GetPreconfiguredEventManagers()
    {
        return new List<EventManager>
            {
                new EventManager { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", PasswordHash = "hashedpassword" },
                new EventManager { FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", PasswordHash = "hashedpassword" },
            };
    }

    private static IEnumerable<Ticket> GetPreconfiguredTickets()
    {
        return new List<Ticket>
            {
                new Ticket { EventId = 1, CustomerId = 1, SeatId = 1, PricesId = 1, VenueId = 1, PurchaseDate = DateTime.Now, IsPrinted = false },
                new Ticket { EventId = 1, CustomerId = 2, SeatId = 2, PricesId = 2, VenueId = 1, PurchaseDate = DateTime.Now, IsPrinted = false },
                new Ticket { EventId = 2, CustomerId = 1, SeatId = 3, PricesId = 3, VenueId = 2, PurchaseDate = DateTime.Now, IsPrinted = false },
                new Ticket { EventId = 2, CustomerId = 2, SeatId = 4, PricesId = 4, VenueId = 2, PurchaseDate = DateTime.Now, IsPrinted = false },
            };
    }
}
