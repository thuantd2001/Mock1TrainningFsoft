using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO> {
                 new VillaDTO {Id = 1 , Name="pool",Sqft=100,Occupancy=4 },
                new VillaDTO {Id = 2 , Name="beach", Sqft = 300, Occupancy = 3 }
        
        };
    }
}
