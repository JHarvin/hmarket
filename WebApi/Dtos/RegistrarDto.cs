using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class RegistrarDto
{
    [Required(ErrorMessage = "El Email es requerido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    public string Username { get; set; }
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "El apellido es requerido")]
    public string Apellido { get; set; }
    [Required(ErrorMessage = "El campo contraseña es requerido")]
    public string Password { get; set; }
}
