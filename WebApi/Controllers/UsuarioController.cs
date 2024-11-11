using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{

    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsuarioController(IMapper mapper,ITokenService tokenService, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("login")]

        public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);
            if (!resultado.Succeeded) {
                return Unauthorized(new CodeErrorResponse(401));
            }

            UsuarioDto userDto = new UsuarioDto();

            userDto.Email = usuario.Email;
            userDto.Username = usuario.UserName;
            userDto.Token = _tokenService.CreateToken(usuario);
            userDto.Nombre = usuario.Nombre;
            userDto.Apellido = usuario.Apellido;

            return Ok(userDto);

        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> Registrar([FromBody] RegistrarDto registrarDto) {

            var usuario = new Usuario() { 
             Email = registrarDto.Email,
             UserName = registrarDto.Username,
             Apellido = registrarDto.Apellido,
             Nombre = registrarDto.Nombre
            };

            // hacer registro
            var ressult = await _userManager.CreateAsync(usuario, registrarDto.Password);
            if (!ressult.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            UsuarioDto  user = new UsuarioDto();
            user.Email = usuario.Email;
            user.Username = usuario.UserName;
            user.Nombre = usuario.Nombre;
            user.Apellido = usuario.Apellido;
            user.Token = _tokenService.CreateToken(usuario);


            return Ok(user);

        
        }

        [HttpGet("GetUsuario")]
        [Authorize]
        public async Task<ActionResult<UsuarioDto>> GetUsuario() {
         

            var user = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            return new UsuarioDto {
             Nombre = user.Nombre,
             Apellido = user.Apellido,
             Email = user.Email,
             Username = user.UserName,
             Token = _tokenService.CreateToken(user)
            };
        }

        [HttpGet("validarMail")]
        public async Task<ActionResult<bool>> ValidarEmail([FromQuery]string email) {
          var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {

                return false;
            }
            return true;
        }

        [Authorize]
        [HttpGet("direccion")]
        public async Task<ActionResult<DireccionDto>> GetDireccion() {

            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);

            return _mapper.Map<Direccion, DireccionDto>( usuario.Direccion);

        }

        [Authorize]
        [HttpPut("actualizarDireccion")]
        public async Task<ActionResult<DireccionDto>> actualizarDireccion([FromBody]DireccionDto direccion) {
            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);
            usuario.Direccion =  _mapper.Map<DireccionDto, Direccion>(direccion);
           var resultado = await _userManager.UpdateAsync(usuario);
            if (resultado.Succeeded) {
                return Ok("Exito"); ;
            }
            return BadRequest("No se pudo actualizar la dirección del usuario");

        }

    }
}
