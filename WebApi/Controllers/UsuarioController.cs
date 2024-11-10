using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
 
    public class UsuarioController : BaseApiController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost("login")]
        
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);
            if (User == null) {
              return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(usuario,loginDto.Password, false);
            if (!resultado.Succeeded) {
                return Unauthorized(new CodeErrorResponse(401));
            }

            UsuarioDto userDto = new UsuarioDto();

            userDto.Email = usuario.Email;
            userDto.Username = usuario.UserName;
            userDto.Token = "Este es el token del usuario";
            userDto.Nombre = usuario.Nombre;
            userDto.Apellido = usuario.Apellido;

            return Ok(userDto);

        }
    }
}
