using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Mock1Trainning.Data;
using Mock1Trainning.Models;
using Mock1Trainning.Models.DTO;
using Mock1Trainning.Models.Response;
using Mock1Trainning.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mock1Trainning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private string secretKey;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public UserController(IUserRepository userRepository, IMapper mapper,IConfiguration configuration)
        {

            _userRepository = userRepository;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            this._response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var user =await _userRepository.Login(loginRequestDTO.UserName, loginRequestDTO.Password);
            if (user == null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess =false;
                _response.ErrorMessage.Add("Username or password incorrect");
                return BadRequest(_response);

            }
            var userDTO = _mapper.Map<UserDTO>(user);
            var jwtTokenhandle = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {

                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),

                    //roles
                    new Claim("TokenId", Guid.NewGuid().ToString())

                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtTokenhandle.CreateToken(tokenDescription);
            var loginResponse =  new LoginResponseDTO
            {
                Token = jwtTokenhandle.WriteToken(token),
                User = userDTO
            };

            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Regitser(RegisterRequestDTO registerRequestDTO)
        {
            var isUnique = _userRepository.IsUniqueUser(registerRequestDTO.UserName);
            if (!isUnique)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Username already exists");
                return BadRequest(_response);
            }
            var register = _mapper.Map<LocalUser>(registerRequestDTO);
            var user = await _userRepository.Register(register);
            if (user == null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessage.Add("Error while registering ");
                return BadRequest(_response);
            }
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);

        }

    }
}
