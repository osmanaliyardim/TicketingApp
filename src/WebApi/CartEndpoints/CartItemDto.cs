using System.ComponentModel.DataAnnotations;

namespace TicketingApp.WebApi.CartEndpoints;

public class CartItemDto
{
    public int Id { get; set; }

    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be bigger than 0")]
    public int Quantity { get; set; }

    public int EventId { get; set; }

    public string EventName { get; set; }
}
