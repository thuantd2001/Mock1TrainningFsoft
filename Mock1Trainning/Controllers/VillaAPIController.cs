using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mock1Trainning.Data;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet("GetAllVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok( VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault( v => v.Id == id);
            if (villa == null) return NotFound();
            return Ok();
        }
        [HttpPost("CreateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
 
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null) return BadRequest(villaDTO);
            if (villaDTO.Id>  0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1; 
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }
    }
}
