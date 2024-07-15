using Microsoft.EntityFrameworkCore;
using TicketingApp.ApplicationCore.Interfaces;

namespace TicketingApp.Infrastructure.Data.Queries;

public class BasketQueryService : IBasketQueryService
{
    private readonly TicketingContext _dbContext;

    public BasketQueryService(TicketingContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CountTotalBasketItems(string username)
    {
        var totalItems = await _dbContext.Baskets
            .Where(basket => basket.BuyerId == username)
            .SelectMany(item => item.Items)
            .SumAsync(sum => sum.Quantity);

        return totalItems;
    }
}
