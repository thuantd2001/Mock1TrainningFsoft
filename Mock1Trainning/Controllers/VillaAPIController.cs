
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Data;
using Mock1Trainning.Logging;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;

namespace Mock1Trainning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        
        private readonly ILogging _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAPIController(ILogging logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet("GetAllVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villalist = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villalist));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("Get villa error with id: " + id,"error");
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null) return NotFound();
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost("CreateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower().Equals(villaDTO.Name)) != null)
            {
                ModelState.AddModelError("Custommer errror", "Villa already exists!");
            }
            if (villaDTO == null) return BadRequest(villaDTO);
            //if (villaDTO.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);

            //}
            //var model = new Villa
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,

            //}
            Villa model = _mapper.Map<Villa>(villaDTO);
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { id = model.Id }, villaDTO);
        }
        [HttpDelete("DeleteVilla/{id:int}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> DeleteVilla(int id)
        {


            if (id == 0)
            {
                return BadRequest();

            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            return NoContent(); ;
        }
        [HttpPut("UpdateVilla/{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {


            if (id == 0)
            {
                return BadRequest();

            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //villa.Name = villaDTO.Name;
            //villa.Sqft = villaDTO.Sqft;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Occupancy = villaDTO.Occupancy;

            Villa model = _mapper.Map<Villa>(villaDTO);

            _db.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("UpdatePartialVilla/{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {


            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);
            if (villa == null)
            {
               return  BadRequest();
            }
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);
            Villa model = _mapper.Map<Villa>(patchDTO);
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
