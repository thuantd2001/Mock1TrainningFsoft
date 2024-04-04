
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mock1Trainning.Data;
using Mock1Trainning.Logging;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;
using Mock1Trainning.Models.Response;
using Mock1Trainning.Repository;

namespace Mock1Trainning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        
        private readonly ILogging _logger;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public VillaAPIController(ILogging logger, IVillaRepository villaRepository, IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaRepository;
            _mapper = mapper;
            this._response = new();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVillas()
        {
            IEnumerable<Villa> villalist = await _villaRepository.GetAll();
            _response.Result = _mapper.Map<List<VillaDTO>>(villalist);

            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(_response);
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
            var villa = _villaRepository.Get(v => v.Id == id, false);
            if (villa == null) return NotFound();
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost("CreateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
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
            _villaRepository.Create(model);
            _response.Result = model;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
         
            return _response;
        }
        [HttpDelete("DeleteVilla/{id:int}")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {


            if (id == 0)
            {
                return BadRequest();

            }
            Villa villa =  await _villaRepository.Get(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            await _villaRepository.Remove(villa);

            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        [Authorize("Admin")]
        [HttpPut("UpdateVilla/{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {


            if (id != villaDTO.Id)
            {
                return BadRequest();

            }
            var villa = _villaRepository.Get(v => v.Id == id);
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

            await _villaRepository.Update(model);

            _response.Result = model;
            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
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
            var villa = _villaRepository.Get(v => v.Id == id,false);
            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);
            if (villa == null)
            {
               return  BadRequest();
            }
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);
            Villa model = _mapper.Map<Villa>(patchDTO);
            
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _villaRepository.Update(model);
            _response.Result = model;
            _response.StatusCode = System.Net.HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
