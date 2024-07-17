using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using TicketingApp.ApplicationCore.Extensions;
using TicketingApp.WebApi.Constants;
using TicketingApp.WebApi.EventEndpoints;

namespace TicketingApp.WebApiIntegrationTests.EventEndpoints;

public class EventListPagedEndpointTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EventListPagedEndpointTest(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsFirst2Events()
    {
        var response = await _client.GetAsync($"{ApiConstants.API_PREFIX}/events");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<ListPagedEventResponse>();

        Assert.IsType<List<EventDto>>(model!.Events);
        Assert.Equal(2, model!.Events.Count);
    }

    [Fact]
    public async Task ReturnsCorrectEventsGivenPageIndex1()
    {
        var pageSize = 2;
        var pageIndex = 1;

        var response = await _client.GetAsync($"{ApiConstants.API_PREFIX}/events");
        response.EnsureSuccessStatusCode();
        var stringResponse = await response.Content.ReadAsStringAsync();
        var model = stringResponse.FromJson<ListPagedEventResponse>();
        var totalItem = model!.Events.Count;

        var response2 = await _client.GetAsync($"{ApiConstants.API_PREFIX}/events?pageSize={pageSize}&pageIndex={pageIndex}");
        response.EnsureSuccessStatusCode();
        var stringResponse2 = await response2.Content.ReadAsStringAsync();
        var model2 = stringResponse2.FromJson<ListPagedEventResponse>();

        var totalExpected = totalItem - (pageSize * pageIndex);

        Assert.Equal(totalExpected, model2!.Events.Count);
        Assert.Equal(2, totalItem);
        Assert.IsType<List<EventDto>>(model.Events);
        Assert.IsType<List<EventDto>>(model2.Events);
    }

    [Theory]
    [InlineData("/events")]
    [InlineData("/events/1/sections/1/seats")]
    public async Task SuccessFullMutipleParallelCall(string endpointName)
    {
        var tasks = new List<Task<HttpResponseMessage>>();

        for (int i = 0; i < 100; i++)
        {
            var task = _client.GetAsync($"{ApiConstants.API_PREFIX}{endpointName}");
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        var totalKO = tasks.Count(t => t.Result.StatusCode != HttpStatusCode.OK);

        Assert.Equal(0, totalKO);
    }
}
