using TicketingApp.ApplicationCore.Entities;
using Moq;

namespace TicketingApp.UnitTests.ApplicationCore.Specifications;

public class EventItemsSpecification
{
    [Fact]
    public void MatchesSpecificCatalogItem()
    {
        // Arrange
        var eventItemIds = new int[] { 1 };

        // Act
        var spec = new TicketingApp.ApplicationCore.Specifications.EventItemsSpecification(eventItemIds);
        var result = spec.Evaluate(GetTestCollection()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.ToList());
    }

    [Fact]
    public void MatchesAllCatalogItems()
    {
        // Arrange
        var eventItemIds = new int[] { 1, 2 };

        // Act
        var spec = new TicketingApp.ApplicationCore.Specifications.EventItemsSpecification(eventItemIds);
        var result = spec.Evaluate(GetTestCollection()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.ToList().Count);
    }

    [Fact]
    public void NoMatchesAllCatalogItems()
    {
        // Arrange
        var eventItemIds = new int[] { 3, 4 };

        // Act
        var spec = new TicketingApp.ApplicationCore.Specifications.EventItemsSpecification(eventItemIds);
        var result = spec.Evaluate(GetTestCollection()).ToList();

        // Assert
        Assert.NotNull(result);
        //Assert.Equal(0, result.ToList().Count);
        Assert.Empty(result);
    }

    private List<Event> GetTestCollection()
    {
        var catalogItems = new List<Event>();

        var mockCatalogItem1 = new Mock<Event>("Event 1 description", "Event 1", DateTime.UtcNow.AddDays(1), TimeSpan.FromHours(2), 1);
        mockCatalogItem1.Setup(item => item.Id).Returns(1);

        var mockCatalogItem3 = new Mock<Event>("Event 2 description", "Event 2", DateTime.UtcNow.AddDays(2), TimeSpan.FromHours(2), 2);
        mockCatalogItem3.Setup(item => item.Id).Returns(2);

        catalogItems.Add(mockCatalogItem1.Object);
        catalogItems.Add(mockCatalogItem3.Object);

        return catalogItems;
    }
}
