using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Country, CountryToCreateDto>().ReverseMap();
            CreateMap<Country, CountryToGetDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryToUpdateDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
 