using AutoMapper;
using WebAPI.Domain.Model.Entities;
using WebAPI.Identity.Models;

namespace WebAPI.Identity.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
        }
    }
}
