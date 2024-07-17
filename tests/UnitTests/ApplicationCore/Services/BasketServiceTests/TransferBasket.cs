using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Services;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Specifications;
using Moq;

namespace TicketingApp.UnitTests.ApplicationCore.Services.BasketServiceTests;

public class TransferBasket
{
    private readonly string _nonexistentAnonymousBasketBuyerId = "nonexistent-anonymous-basket-buyer-id";
    private readonly string _existentAnonymousBasketBuyerId = "existent-anonymous-basket-buyer-id";
    private readonly string _nonexistentUserBasketBuyerId = "newuser@microsoft.com";
    private readonly string _existentUserBasketBuyerId = "testuser@microsoft.com";
    private readonly Mock<IRepository<Basket>> _mockBasketRepo = new Mock<IRepository<Basket>>();
    private readonly Mock<IAppLogger<BasketService>> _mockLogger = new Mock<IAppLogger<BasketService>>();

    public class Results<T>
    {
        private readonly Queue<Func<T>> values = new Queue<Func<T>>();
        public Results(T result) { values.Enqueue(() => result); }
        public Results<T> Then(T value) { return Then(() => value); }
        public Results<T> Then(Func<T> value)
        {
            values.Enqueue(value);
            return this;
        }
        public T Next() { return values.Dequeue()(); }
    }

    [Fact]
    public async Task InvokesBasketRepositoryFirstOrDefaultAsyncOnceIfAnonymousBasketNotExists()
    {
        // Arrange
        var anonymousBasket = null as Basket;
        var userBasket = new Basket(_existentUserBasketBuyerId);

        var results = new Results<Basket?>(anonymousBasket).Then(userBasket);

        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(() => results.Next());

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.TransferBasketAsync(_nonexistentAnonymousBasketBuyerId, _existentUserBasketBuyerId);

        // Assert
        _mockBasketRepo.Verify(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default), Times.Once);
    }

    [Fact]
    public async Task TransferAnonymousBasketItemsWhilePreservingExistingUserBasketItems()
    {
        // Arrange
        var anonymousBasket = new Basket(_existentAnonymousBasketBuyerId);
        anonymousBasket.AddItem(1, 10, 1);
        anonymousBasket.AddItem(3, 55, 7);
        var userBasket = new Basket(_existentUserBasketBuyerId);
        userBasket.AddItem(1, 10, 4);
        userBasket.AddItem(2, 99, 3);

        var results = new Results<Basket>(anonymousBasket).Then(userBasket);

        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(() => results.Next());

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.TransferBasketAsync(_nonexistentAnonymousBasketBuyerId, _existentUserBasketBuyerId);

        // Assert
        _mockBasketRepo.Verify(repo => repo.UpdateAsync(userBasket, default), Times.Once);
        Assert.Equal(3, userBasket.Items.Count);
        Assert.Contains(userBasket.Items, x => x.EventId == 1 && x.UnitPrice == 10 && x.Quantity == 5);
        Assert.Contains(userBasket.Items, x => x.EventId == 2 && x.UnitPrice == 99 && x.Quantity == 3);
        Assert.Contains(userBasket.Items, x => x.EventId == 3 && x.UnitPrice == 55 && x.Quantity == 7);
    }

    [Fact]
    public async Task RemovesAnonymousBasketAfterUpdatingUserBasket()
    {
        // Arrange
        var anonymousBasket = new Basket(_existentAnonymousBasketBuyerId);
        var userBasket = new Basket(_existentUserBasketBuyerId);

        var results = new Results<Basket>(anonymousBasket).Then(userBasket);

        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(() => results.Next());

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.TransferBasketAsync(_nonexistentAnonymousBasketBuyerId, _existentUserBasketBuyerId);

        // Assert
        _mockBasketRepo.Verify(repo => repo.UpdateAsync(userBasket, default), Times.Once);
        _mockBasketRepo.Verify(repo => repo.DeleteAsync(anonymousBasket, default), Times.Once);
    }

    [Fact]
    public async Task CreatesNewUserBasketIfNotExists()
    {
        // Arrange
        var anonymousBasket = new Basket(_existentAnonymousBasketBuyerId);
        var userBasket = null as Basket;

        var results = new Results<Basket?>(anonymousBasket).Then(userBasket);

        _mockBasketRepo.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<BasketWithItemsSpecification>(), default))
            .ReturnsAsync(() => results.Next());

        // Act
        var basketService = new BasketService(_mockBasketRepo.Object, _mockLogger.Object);
        await basketService.TransferBasketAsync(_existentAnonymousBasketBuyerId, _nonexistentUserBasketBuyerId);

        // Assert
        _mockBasketRepo.Verify(repo => repo.AddAsync(It.Is<Basket>(x => x.BuyerId == _nonexistentUserBasketBuyerId), default), Times.Once);
    }
}

