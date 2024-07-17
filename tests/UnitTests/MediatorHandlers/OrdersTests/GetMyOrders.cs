using Ardalis.Specification;
using Moq;
using TicketingApp.ApplicationCore.Entities.OrderAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.WebApi.Features.MyOrders;

namespace TicketingApp.UnitTests.MediatorHandlers.OrdersTests;

public class GetMyOrders
{
    private readonly Mock<IReadRepository<Order>> _mockOrderRepository = new Mock<IReadRepository<Order>>();

    public GetMyOrders()
    {
        var item = new OrderItem(new EventOrdered(1, "EventName"), 10.00m, 10);
        var address = new Address("Ataturk St.", "Izmir", "35", "Turkiye", "35530");
        Order order = new Order("customer@ticketing.com", address, new List<OrderItem> { item });

        _mockOrderRepository.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Order>>(), default))
            .ReturnsAsync(new List<Order> { order });
    }


    [Fact]
    public async Task NotReturnNullIfOrdersArePresent()
    {
        var request = new TicketingApp.WebApi.Features.MyOrders.GetMyOrders("SomeUserName");

        var handler = new GetMyOrdersHandler(_mockOrderRepository.Object);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result);
    }
}
