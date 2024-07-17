using TicketingApp.ApplicationCore.Entities;

namespace TicketingApp.UnitTests.ApplicationCore.Specifications;

public class EventFilterSpecification
{
    [Theory]
    [InlineData(null, 5)]
    [InlineData(1, 3)]
    [InlineData(2, 2)]
    [InlineData(3, 0)]
    [InlineData(-1, 0)]
    public void MatchesExpectedNumberOfItems(int? venueId, int expectedCount)
    {
        // Arrange
        var spec = new TicketingApp.ApplicationCore.Specifications.EventFilterSpecification(venueId);

        // Act
        var result = spec.Evaluate(GetTestItemCollection()).ToList();

        // Assert
        Assert.Equal(expectedCount, result.Count());
    }

    public List<Event> GetTestItemCollection()
    {
        return new List<Event>()
            {
                new Event("Event 1 description", "Event 1", DateTime.UtcNow.AddDays(1), TimeSpan.FromHours(2), 1),
                new Event("Event 2 description", "Event 2", DateTime.UtcNow.AddDays(2), TimeSpan.FromHours(3), 1),
                new Event("Event 3 description", "Event 3", DateTime.UtcNow.AddDays(3), TimeSpan.FromHours(2), 1),
                new Event("Event 4 description", "Event 4", DateTime.UtcNow.AddDays(4), TimeSpan.FromHours(1), 2),
                new Event("Event 5 description", "Event 5", DateTime.UtcNow.AddDays(5), TimeSpan.FromHours(4), 2)
            };
    }
}
