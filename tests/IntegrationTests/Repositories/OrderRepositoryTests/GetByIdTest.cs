using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.Infrastructure.Data;
using TicketingApp.UnitTests.Builders;
using Xunit.Abstractions;

namespace TicketingApp.IntegrationTests.Repositories.OrderRepositoryTests;

public class GetByIdTest
{
    private readonly TicketingContext _ticketingContext;
    private readonly EfRepository<Order> _orderRepository;

    private OrderBuilder OrderBuilder { get; } = new OrderBuilder();
    private readonly ITestOutputHelper _output;

    public GetByIdTest(ITestOutputHelper output)
    {
        _output = output;
        var dbOptions = new DbContextOptionsBuilder<TicketingContext>()
            .UseInMemoryDatabase(databaseName: "TicketingDB")
            .Options;

        _ticketingContext = new TicketingContext(dbOptions);
        _orderRepository = new EfRepository<Order>(_ticketingContext);
    }

    [Fact]
    public async Task GetsExistingOrder()
    {
        var existingOrder = OrderBuilder.WithDefaultValues();
        _ticketingContext.Orders.Add(existingOrder);
        _ticketingContext.SaveChanges();
        int orderId = existingOrder.Id;
        _output.WriteLine($"OrderId: {orderId}");

        var orderFromRepo = await _orderRepository.GetByIdAsync(orderId);
        Assert.Equal(OrderBuilder.TestBuyerId, orderFromRepo.BuyerId);

        // Note: Using InMemoryDatabase OrderItems is available. Will be null if using SQL DB.
        // Use the OrderWithItemsByIdSpec instead of just GetById to get the full aggregate
        var firstItem = orderFromRepo.OrderItems.FirstOrDefault();
        Assert.Equal(OrderBuilder.TestUnits, firstItem.Units);
    }
}
