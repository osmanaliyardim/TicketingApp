using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Services;
using TicketingApp.ApplicationCore.Interfaces;
using Moq;

namespace TicketingApp.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class DeleteBasket
{
    private readonly string _buyerId = "customer@ticketing.com";
    private readonly Mock<IRepository<Basket>> _mockBasketRepo = new Mock<IRepository<Basket>>();
    private readonly Mock<IAppLogger<BasketService>> _mockLogger = new Mock<IAppLogger<BasketService>>();

    [Fact]
    public async Task ShouldInvokeBasketRepositoryDeleteAsyncOnce()
    {
        // Arrange
        var basket = new Basket(_buyerId);
        basket.AddItem(1, 1.1m, 1);
        basket.AddItem(2, 1.1m, 1);
        _mockBasketRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), default))
            .ReturnsAsync(basket);

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.DeleteBasketAsync(1);

        // Assert
        _mockBasketRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Basket>(), default), Times.Once);
    }
}
