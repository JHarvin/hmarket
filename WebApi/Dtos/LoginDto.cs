using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class LoginDto
{
    [Required(ErrorMessage ="El Email es requerido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "La contraseña es requerido")]
    public string Password { get; set; }
}
