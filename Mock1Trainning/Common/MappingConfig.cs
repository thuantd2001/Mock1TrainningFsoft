using AutoMapper;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Common
{
    public class MappingConfig : Profile

    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
           
        }
    }
}
