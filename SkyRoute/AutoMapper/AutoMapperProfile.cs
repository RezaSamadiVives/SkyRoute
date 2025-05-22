using AutoMapper;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.ViewModels;

namespace SkyRoute.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {

            CreateMap<Airline, AirlineVM>();
            CreateMap<City, CityVM>();
            CreateMap<Flight, FlightVM>();
            CreateMap<FlightMealOption, FlightMealOptionVM>();
            CreateMap<FlightRoute, FlightRouteVM>();
            CreateMap<MealOption, MealOptionVM>();
            CreateMap<RouteStopover, RouteStopoverVM>();
            CreateMap<Seat, SeatVM>();
            CreateMap<FlightSearchResult, FlightSearchResultVM>();
            CreateMap<FlightSegmentGroup, FlightSegmentGroupVM>();
        
        }
    }
}
