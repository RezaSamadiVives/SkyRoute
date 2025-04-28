using AutoMapper;
using SkyRoute.Domains.Entities;
using SkyRoute.ViewModels;

namespace SkyRoute.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {

            CreateMap<Airline, AirlineVM>();
        
        }
    }
}
