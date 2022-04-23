using AutoMapper;
using System;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Configurations
{
    public class MapperInitilizer:Profile 
    {
        public MapperInitilizer()
        {
            CreateMap<Country,CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

            CreateMap<ApiUser, UserDto>().ReverseMap();
            CreateMap<ApiUser, LoginDto>().ReverseMap();
            CreateMap<LoginDto, UserDto>().ReverseMap();

        }
    }
}
