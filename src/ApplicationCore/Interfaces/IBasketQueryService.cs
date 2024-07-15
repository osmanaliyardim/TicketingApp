namespace TicketingApp.ApplicationCore.Interfaces;

public interface IBasketQueryService
{
    Task<int> CountTotalBasketItems(string username);
}
