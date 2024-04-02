using AutoMapper;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Common
{
    public class MappingConfig : Profile

    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, Villa>().ReverseMap();
            CreateMap<VillaDTO, Villa>().ReverseMap();
           
        }
    }
}
