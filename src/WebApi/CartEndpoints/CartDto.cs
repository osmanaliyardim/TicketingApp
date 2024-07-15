namespace TicketingApp.WebApi.CartEndpoints;

public record CartDto
{
    public int Id { get; set; }

    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

    public string? BuyerId { get; set; }

    public decimal TotalPrice => Total();

    public decimal Total()
    {
        return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
    }
}
