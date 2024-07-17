using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.Infrastructure.Data;
using TicketingApp.UnitTests.Builders;

namespace TicketingApp.IntegrationTests.Repositories.OrderRepositoryTests;

public class GetByIdWithItemsAsyncTest
{
    private readonly TicketingContext _ticketingContext;
    private readonly EfRepository<Order> _orderRepository;

    private OrderBuilder OrderBuilder { get; } = new OrderBuilder();

    public GetByIdWithItemsAsyncTest()
    {
        var dbOptions = new DbContextOptionsBuilder<TicketingContext>()
            .UseInMemoryDatabase(databaseName: "TicketingDB")
            .Options;

        _ticketingContext = new TicketingContext(dbOptions);
        _orderRepository = new EfRepository<Order>(_ticketingContext);
    }

    [Fact]
    public async Task GetOrderAndItemsByOrderIdWhenMultipleOrdersPresent()
    {
        //Arrange
        var itemOneUnitPrice = 5.50m;
        var itemOneUnits = 2;
        var itemTwoUnitPrice = 7.50m;
        var itemTwoUnits = 5;

        var firstOrder = OrderBuilder.WithDefaultValues();
        _ticketingContext.Orders.Add(firstOrder);
        int firstOrderId = firstOrder.Id;

        var secondOrderItems = new List<OrderItem>();
        secondOrderItems.Add(new OrderItem(OrderBuilder.TestEventOrdered, itemOneUnitPrice, itemOneUnits));
        secondOrderItems.Add(new OrderItem(OrderBuilder.TestEventOrdered, itemTwoUnitPrice, itemTwoUnits));
        var secondOrder = OrderBuilder.WithItems(secondOrderItems);
        _ticketingContext.Orders.Add(secondOrder);
        int secondOrderId = secondOrder.Id;

        _ticketingContext.SaveChanges();

        //Act
        var spec = new OrderWithItemsByIdSpec(secondOrderId);
        var orderFromRepo = await _orderRepository.FirstOrDefaultAsync(spec);

        //Assert
        Assert.Equal(secondOrderId, orderFromRepo.Id);
        Assert.Equal(secondOrder.OrderItems.Count, orderFromRepo.OrderItems.Count);
        Assert.Equal(1, orderFromRepo.OrderItems.Count(x => x.UnitPrice == itemOneUnitPrice));
        Assert.Equal(1, orderFromRepo.OrderItems.Count(x => x.UnitPrice == itemTwoUnitPrice));
        Assert.Equal(itemOneUnits, orderFromRepo.OrderItems.SingleOrDefault(x => x.UnitPrice == itemOneUnitPrice).Units);
        Assert.Equal(itemTwoUnits, orderFromRepo.OrderItems.SingleOrDefault(x => x.UnitPrice == itemTwoUnitPrice).Units);
    }
}
