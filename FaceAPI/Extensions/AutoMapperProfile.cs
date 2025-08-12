using AutoMapper;
using FaceAPI.Model.SugarEntity;
using Shared;

namespace FaceAPI.Extensions
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<T_USER, UserDto>().ReverseMap();
            CreateMap<T_FACE, FaceDto>().ReverseMap();
        }
    }
}
