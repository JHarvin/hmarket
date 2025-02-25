using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class ProductoUpdateDto
{
    [Required(ErrorMessage = "El Id del producto es requerido para actualizar")]
    public int Id { get; set; }
    [Required(ErrorMessage = "El Nombre del producto es requerido")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "La descripción del producto es requerido")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "El Stock del producto es requerido")]
    public int Stock { get; set; }
    [Required(ErrorMessage = "El Id de la marca es requerido")]
    public int MarcaId { get; set; }

    [Required(ErrorMessage = "El Id de la categoría es requerido")]
    public int CategoriaId { get; set; }
   
    [Required(ErrorMessage = "El precio es requerido")]
    public decimal Precio { get; set; }
    
    public IFormFile Foto { get; set; } // foto
    
}
