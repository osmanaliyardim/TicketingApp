using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Entities.BuyerAggregate;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;

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
            if (!await ticketingContext.Venues.AnyAsync())
            {
                await ticketingContext.Venues.AddRangeAsync(GetPreconfiguredVenues());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Sections.AnyAsync())
            {
                await ticketingContext.Sections.AddRangeAsync(GetPreconfiguredSections());
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

            //if (!await ticketingContext.PaymentMethods.AnyAsync())
            //{
            //    await ticketingContext.PaymentMethods.AddRangeAsync(GetPreconfiguredPaymentMethods());
            //    await ticketingContext.SaveChangesAsync();
            //}

            if (!await ticketingContext.Buyers.AnyAsync())
            {
                await ticketingContext.Buyers.AddRangeAsync(GetPreconfiguredBuyers());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.EventManagers.AnyAsync())
            {
                await ticketingContext.EventManagers.AddRangeAsync(GetPreconfiguredEventManagers());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Tickets.AnyAsync())
            {
                await ticketingContext.Tickets.AddRangeAsync(GetPreconfiguredTickets(ticketingContext));
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.Baskets.AnyAsync())
            {
                await ticketingContext.Baskets.AddRangeAsync(GetPreconfiguredBaskets());
                await ticketingContext.SaveChangesAsync();
            }

            if (!await ticketingContext.BasketItems.AnyAsync())
            {
                await ticketingContext.BasketItems.AddRangeAsync(GetPreconfiguredBasketItems());
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

    private static IEnumerable<Basket> GetPreconfiguredBaskets()
    {
        return new List<Basket>
            {
                new Basket("1")
            };
    }

    private static IEnumerable<BasketItem> GetPreconfiguredBasketItems()
    {
        return new List<BasketItem>
            {
                new BasketItem(1, 2, 10, 1015)
            };
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
                new Event("Concert X by Band Y", "Concert X", new DateTime(2024, 8, 1), new TimeSpan(19, 0, 0), 1),
                new Event("Game Z Championship", "Sports Game Z", new DateTime(2024, 8, 15), new TimeSpan(18, 0, 0), 2)
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

    //private static IEnumerable<PaymentMethod> GetPreconfiguredPaymentMethods()
    //{
    //    return new List<PaymentMethod>
    //        {
    //             new PaymentMethod("777", "1234567891234567", "4567"),
    //             new PaymentMethod("555", "1231237891237890", "7890"),
    //             new PaymentMethod("666", "4564567891233456", "3456")
    //        };
    //}

    private static IEnumerable<Buyer> GetPreconfiguredBuyers()
    {
        var PaymentMethodsForBuyer1 = new List<PaymentMethod> { new PaymentMethod("777", "1234567891234567", "4567") };
        var PaymentMethodsForBuyer2 = new List<PaymentMethod>
        {
            new PaymentMethod("555", "1231237891237890", "7890"),
            new PaymentMethod("666", "4564567891233456", "3456")
        };

        return new List<Buyer>
            {
                new Buyer(Guid.NewGuid().ToString(), PaymentMethodsForBuyer1),
                new Buyer(Guid.NewGuid().ToString(), PaymentMethodsForBuyer2),
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

    private static IEnumerable<Ticket> GetPreconfiguredTickets(TicketingContext ticketingContext)
    {
        return new List<Ticket>
            {
                new Ticket { EventId = 1, BuyerId = 1, PricesId = 1, VenueId = 1, PurchaseDate = DateTime.Now, IsPrinted = false, Seat = ticketingContext.Seats.FirstOrDefault(s => s.Id == 1) },
                new Ticket { EventId = 2, BuyerId = 2, PricesId = 2, VenueId = 2, PurchaseDate = DateTime.Now, IsPrinted = false, Seat = ticketingContext.Seats.FirstOrDefault(s => s.Id == 8) },
                new Ticket { EventId = 1, BuyerId = 1, PricesId = 3, VenueId = 1, PurchaseDate = DateTime.Now, IsPrinted = false, Seat = ticketingContext.Seats.FirstOrDefault(s => s.Id == 3) },
                new Ticket { EventId = 2, BuyerId = 2, PricesId = 4, VenueId = 2, PurchaseDate = DateTime.Now, IsPrinted = false, Seat = ticketingContext.Seats.FirstOrDefault(s => s.Id == 9) }
            };
    }

    private static IEnumerable<Seat> GetPreconfiguredSeats()
    {
        return new List<Seat>
            {
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "A", Number = 1, IsAvailable = true, SectionId = 1 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "B", Number = 1, IsAvailable = false, SectionId = 1 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "B", Number = 2, IsAvailable = true, SectionId = 3 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "C", Number = 3, IsAvailable = true, SectionId = 5 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "A", Number = 2, IsAvailable = true, SectionId = 5 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "B", Number = 1, IsAvailable = false, SectionId = 5 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "D", Number = 4, IsAvailable = true, SectionId = 7 },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "E", Number = 5, IsAvailable = true, SectionId = 2 },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "F", Number = 6, IsAvailable = true, SectionId = 4 },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "G", Number = 7, IsAvailable = true, SectionId = 6 },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "H", Number = 8, IsAvailable = true, SectionId = 8 },
                new Seat { ManifestId = 2, SeatType = SeatTypes.Row, Row = "J", Number = 9, IsAvailable = true, SectionId = 9 },
                new Seat { ManifestId = 1, SeatType = SeatTypes.GeneralAdmission, Row = null, Number = 5, SectionId = 10, IsAvailable = true },
                new Seat { ManifestId = 2, SeatType = SeatTypes.GeneralAdmission, Row = null, Number = 6, SectionId = 11, IsAvailable = true },
                new Seat { ManifestId = 2, SeatType = SeatTypes.GeneralAdmission, Row = null, Number = 7, SectionId = 11, IsAvailable = false }
            };
    }

    private static IEnumerable<Section> GetPreconfiguredSections()
    {
        return new List<Section>
            {
                new Section { Name = "Section-A", VenueId = 1 },
                new Section { Name = "Section-A", VenueId = 2 },
                new Section { Name = "Section-B", VenueId = 1 },
                new Section { Name = "Section-B", VenueId = 2 },
                new Section { Name = "Section-C", VenueId = 1 },
                new Section { Name = "Section-C", VenueId = 2 },
                new Section { Name = "Section-D", VenueId = 1 },
                new Section { Name = "Section-D", VenueId = 2 },
                new Section { Name = "Section-E", VenueId = 2 },
                new Section { Name = "Section-VIP", VenueId = 1 },
                new Section { Name = "Section-VIP", VenueId = 2 },
            };
    }
}
