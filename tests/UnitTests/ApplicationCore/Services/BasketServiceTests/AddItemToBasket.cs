using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Services;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Specifications;
using Moq;

namespace TicketingApp.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class AddItemToBasket
{
    private readonly string _buyerId = "customer@ticketing.com";
    private readonly Mock<IRepository<Basket>> _mockBasketRepo = new Mock<IRepository<Basket>>();
    private readonly Mock<IAppLogger<BasketService>> _mockLogger = new Mock<IAppLogger<BasketService>>();

    [Fact]
    public async Task InvokesBasketRepositoryGetBySpecAsyncOnce()
    {
        // Arrange
        var basket = new Basket(_buyerId);
        basket.AddItem(1, 1.5m);

        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(basket);

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.AddItemToBasket(basket.BuyerId, 1, 1.50m);

        // Assert
        _mockBasketRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default), Times.Once);
    }

    [Fact]
    public async Task InvokesBasketRepositoryUpdateAsyncOnce()
    {
        // Arrange
        var basket = new Basket(_buyerId);
        basket.AddItem(1, 1.1m, 1);
        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(basket);

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.AddItemToBasket(basket.BuyerId, 1, 1.50m);

        // Assert
        _mockBasketRepo.Verify(repo => repo.UpdateAsync(basket, default), Times.Once);
    }
}

