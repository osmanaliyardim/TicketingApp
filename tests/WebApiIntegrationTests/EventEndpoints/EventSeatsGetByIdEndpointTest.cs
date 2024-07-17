using System.Net;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.WebApi.Constants;
using TicketingApp.WebApi.EventEndpoints;
using TicketingApp.ApplicationCore.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TicketingApp.WebApiIntegrationTests.EventEndpoints;

public class EventSeatsGetByIdEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EventSeatsGetByIdEndpointTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsItemGivenValidId()
    {
        // Arrange
        var response = await _client.GetAsync(ApiConstants.API_PREFIX + "/events/1/sections/1/seats");
        response.EnsureSuccessStatusCode();
        var expectedResult = new List<SeatDto>()
        {
            new SeatDto { ManifestId = 1, SeatType = SeatTypes.Row, Row = "A", Number = 1, IsAvailable = true, SectionId = 1 },
            new SeatDto { ManifestId = 1, SeatType = SeatTypes.Row, Row = "B", Number = 1, IsAvailable = false, SectionId = 1 }
        };

        // Act
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<GetByIdEventSeatsResponse>();

        // Assert
        Assert.IsType<List<SeatDto>>(model!.Seats);
        Assert.Equal(expectedResult, model!.Seats);
        Assert.Equal(1, model!.Seats.First().Number);
        Assert.Equal(1, model!.Seats.Last().Number);
    }

    [Fact]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        var response = await _client.GetAsync(ApiConstants.API_PREFIX + "/events/88/sections/77/seats");

        Assert.Equal(HttpStatusCode.NotFound, response!.StatusCode);
    }
}
