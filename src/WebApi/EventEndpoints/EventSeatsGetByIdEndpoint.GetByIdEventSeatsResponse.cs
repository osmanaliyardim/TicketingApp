namespace TicketingApp.WebApi.EventEndpoints;

public class GetByIdEventSeatsResponse : BaseResponse
{
    public GetByIdEventSeatsResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetByIdEventSeatsResponse()
    {
    }

    public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
}
