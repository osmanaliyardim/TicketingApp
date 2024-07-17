using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Services;
using TicketingApp.Infrastructure.Data;
using TicketingApp.UnitTests.Builders;

namespace TicketingApp.IntegrationTests.Repositories.BasketRepositoryTests;

public class SetQuantitiesTest
{
    private readonly TicketingContext _ticketingContext;
    private readonly EfRepository<Basket> _basketRepository;
    private readonly BasketBuilder BasketBuilder = new BasketBuilder();

    public SetQuantitiesTest()
    {
        var dbOptions = new DbContextOptionsBuilder<TicketingContext>()
            .UseInMemoryDatabase(databaseName: "TicketingDB")
            .Options;

        _ticketingContext = new TicketingContext(dbOptions);
        _basketRepository = new EfRepository<Basket>(_ticketingContext);
    }

    [Fact]
    public async Task RemoveEmptyQuantities()
    {
        var basket = BasketBuilder.WithOneBasketItem();
        var basketService = new BasketService(_basketRepository, null);
        await _basketRepository.AddAsync(basket);
        _ticketingContext.SaveChanges();

        await basketService.SetQuantities(BasketBuilder.BasketId, new Dictionary<string, int>() { { BasketBuilder.BasketId.ToString(), 0 } });

        Assert.Equal(0, basket.Items.Count);
    }
}
