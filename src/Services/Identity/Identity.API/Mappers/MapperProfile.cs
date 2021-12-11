using AutoMapper;
using Identity.API.Payloads.v1.Requests;
using Identity.API.Models;

namespace Identity.API.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Requests -> Models
            CreateMap<AccountRegisterRequest, User>();
            CreateMap<AccountLoginRequest, User>();
        }
    }
}