﻿using AutoMapper;
using Simple.Treavor.Domain.Model;
using Simple.Treavor.Infrastructure.Data;

namespace Simple.Treavor.Domain.Configurations
{
  public class MapperInitializer : Profile
  {
    public MapperInitializer() 
    {
      CreateMap<Country, CountryDTO>().ReverseMap();
      CreateMap<Country, CreateCountryDTO>().ReverseMap();
      CreateMap<Hotel, HotelDTO>().ReverseMap();
      CreateMap<Hotel, CreateHotelDTO>().ReverseMap();
      CreateMap<ApiUser, UserDTO>().ReverseMap();
    }
  }
}
