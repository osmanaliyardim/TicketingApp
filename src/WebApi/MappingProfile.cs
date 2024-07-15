using AutoMapper;
using TicketingApp.ApplicationCore.Entities;
using TicketingApp.ApplicationCore.Entities.BasketAggregate;
using TicketingApp.WebApi.CartEndpoints;
using TicketingApp.WebApi.EventEndpoints;
using TicketingApp.WebApi.VenueEndpoints;

namespace TicketingApp.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Venue, VenueDto>()
            .ReverseMap();
        
        CreateMap<Event, EventDto>()
            .ReverseMap();

        CreateMap<Seat, SeatDto>()
            .ReverseMap();

        CreateMap<Section, SectionDto>()
            .ReverseMap();

        CreateMap<Basket, CartDto>()
            .ForMember<string>(dest => dest.BuyerId, opts => opts.MapFrom(src => src.BuyerId))
            .ForMember(dest => dest.Items, opts => opts.MapFrom(src => src.Items));

        CreateMap<BasketItem, CartItemDto>()
            .ForMember(dest => dest.UnitPrice, opts => opts.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Quantity, opts => opts.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.EventId, opts => opts.MapFrom(src => src.EventId))
            .ForMember(dest => dest.EventName, opts => opts.Ignore());
    }
}
