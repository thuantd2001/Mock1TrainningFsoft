using AutoMapper;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Common
{
    public class MappingConfig : Profile

    {
        public MappingConfig()
        {
            CreateMap<LocalUser, UserDTO>().ReverseMap();
            CreateMap<LocalUser, RegisterRequestDTO>().ReverseMap();
            CreateMap<LocalUser, LoginRequestDTO>().ReverseMap();
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
          

        }
    }
}
