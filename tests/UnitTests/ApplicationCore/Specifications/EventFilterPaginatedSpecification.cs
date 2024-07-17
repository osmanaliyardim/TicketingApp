using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.UnitTests.ApplicationCore.Specifications;

public class EventFilterPaginatedSpecification
{
    [Fact]
    public void ReturnsAllCatalogItems()
    {
        // Arrange
        var spec = new TicketingApp.ApplicationCore.Specifications.EventFilterPaginatedSpecification(0, 10, null);

        // Act
        var result = spec.Evaluate(GetTestCollection());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, result.ToList().Count);
    }

    [Fact]
    public void Returns2CatalogItemsWithSameBrandAndTypeId()
    {
        // Arrange
        var spec = new TicketingApp.ApplicationCore.Specifications.EventFilterPaginatedSpecification(0, 10, 1);

        // Act
        var result = spec.Evaluate(GetTestCollection()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.ToList().Count);
    }

    private List<Event> GetTestCollection()
    {
        var catalogItemList = new List<Event>();

        catalogItemList.Add(new Event("Event 1 description", "Event 1", DateTime.UtcNow.AddDays(1), TimeSpan.FromHours(2), 1));
        catalogItemList.Add(new Event("Event 2 description", "Event 2", DateTime.UtcNow.AddDays(2), TimeSpan.FromHours(3), 1));
        catalogItemList.Add(new Event("Event 3 description", "Event 3", DateTime.UtcNow.AddDays(3), TimeSpan.FromHours(2), 2));
        catalogItemList.Add(new Event("Event 4 description", "Event 4", DateTime.UtcNow.AddDays(4), TimeSpan.FromHours(1), 2));

        return catalogItemList;
    }
}
