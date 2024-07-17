using Ardalis.Specification;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Interfaces;
using TicketingApp.ApplicationCore.Specifications;
using TicketingApp.WebApi.EventEndpoints;

namespace TicketingApp.UnitTests.WebApi.Endpoints;

public class EventEndpoints
{
    private readonly Mock<IRepository<Event>> _eventRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EventListPagedEndpoint _endpoint;
    private readonly Mock<IRepository<Seat>> _seatRepositoryMock;
    private readonly EventSeatsGetByIdEndpoint _eventSeatsEndpoint;

    public EventEndpoints()
    {
        _eventRepositoryMock = new Mock<IRepository<Event>>();
        _seatRepositoryMock = new Mock<IRepository<Seat>>();
        _mapperMock = new Mock<IMapper>();
        _endpoint = new EventListPagedEndpoint(_mapperMock.Object);
        _eventSeatsEndpoint = new EventSeatsGetByIdEndpoint(_mapperMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ReturnsOkResult_WithExpectedResponse()
    {
        // Arrange
        var request = new ListPagedEventRequest(10, 1, 1);
        var events = new List<Event> { new Event("Event 1 description", "Event 1", DateTime.UtcNow.AddDays(1), TimeSpan.FromHours(2), 1) };
        var eventDtos = events.Select(e => new EventDto { Name = e.Name, Date = e.Date, Description = e.Description, Time = e.Time, VenueId = e.VenueId }).ToList();
        _eventRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<EventFilterSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(events.Count);
        _eventRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<EventFilterPaginatedSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(events);
        _mapperMock.Setup(m => m.Map<EventDto>(It.IsAny<Event>()))
            .Returns((Event src) => 
                new EventDto { 
                    Name = src.Name, Date = src.Date, 
                    Description = src.Description, 
                    Time = src.Time, VenueId = src.VenueId 
                });

        // Act
        var result = await _endpoint.HandleAsync(request, _eventRepositoryMock.Object);
        var okResult = result as Ok<ListPagedEventResponse>;
        var response = okResult.Value;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(eventDtos.Count, response.Events.Count);
        Assert.Equal(eventDtos[0].Name, response.Events[0].Name);
    }

    [Fact]
    public async Task HandleAsync_SetsPageCountCorrectly()
    {
        // Arrange
        var request = new ListPagedEventRequest(10, 1, 1);
        _eventRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<EventFilterSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(25);
        _eventRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<EventFilterPaginatedSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Event>());

        // Act
        var result = await _endpoint.HandleAsync(request, _eventRepositoryMock.Object);
        var okResult = result as Ok<ListPagedEventResponse>;
        var response = okResult.Value;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(3, response.PageCount); // 25 items, 10 per page -> 3 pages
    }

    [Fact]
    public async Task HandleAsync_ReturnsZeroPageCount_WhenNoItems()
    {
        // Arrange
        var request = new ListPagedEventRequest(10, 1, 1);
        _eventRepositoryMock.Setup(repo => repo.CountAsync(It.IsAny<EventFilterSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        _eventRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<EventFilterPaginatedSpecification>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Event>());

        // Act
        var result = await _endpoint.HandleAsync(request, _eventRepositoryMock.Object);
        var okResult = result as Ok<ListPagedEventResponse>;
        var response = okResult.Value;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(0, response.PageCount);
        Assert.Empty(response.Events);
    }

    [Fact]
    public async Task HandleAsync_EventSeats_ReturnsOkResult_WithExpectedResponse()
    {
        // Arrange
        var request = new GetByIdEventSeatsRequest(1, 1);
        var seats = new List<Seat> { new Seat { ManifestId = 1, SeatType = SeatTypes.Row, Row = "A", Number = 1, IsAvailable = true, SectionId = 1 } };
        var seatDtos = seats.Select(s => 
            new SeatDto { 
                Number = s.Number, IsAvailable = s.IsAvailable, 
                ManifestId = s.ManifestId, Row = s.Row, 
                SeatType = s.SeatType, SectionId = s.SectionId }).ToList();
        _seatRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<SeatFilterBySectorIdSpec>(), It.IsAny<CancellationToken>())).ReturnsAsync(seats);
        _mapperMock.Setup(m => m.Map<SeatDto>(It.IsAny<Seat>()))
            .Returns((Seat src) => 
                new SeatDto { 
                    Number = src.Number, IsAvailable = src.IsAvailable, 
                    ManifestId = src.ManifestId, Row = src.Row, 
                    SeatType = src.SeatType, SectionId = src.SectionId 
                });

        // Act
        var result = await _eventSeatsEndpoint.HandleAsync(request, _seatRepositoryMock.Object);
        var okResult = result as Ok<GetByIdEventSeatsResponse>;
        var response = okResult.Value;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(seatDtos.Count, response.Seats.Count);
        Assert.Equal(seatDtos[0].Number, response.Seats[0].Number);
    }

    [Fact]
    public async Task HandleAsync_ReturnsNotFound_WhenNoSeatsFound()
    {
        // Arrange
        var request = new GetByIdEventSeatsRequest(1, 1);
        _seatRepositoryMock.Setup(repo => repo.ListAsync(It.IsAny<SeatFilterBySectorIdSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((List<Seat>)null);

        // Act
        var result = await _eventSeatsEndpoint.HandleAsync(request, _seatRepositoryMock.Object);
        var notFoundResult = result as NotFound;

        // Assert
        Assert.NotNull(notFoundResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
