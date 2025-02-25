using Core.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;
/// <summary>
/// DTO para agregar y actualizar
/// </summary>
public class ProductoDto
{

    
    [Required(ErrorMessage = "El Nombre es requerido")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "La descripción es requerido")]
    public string Descripcion { get; set; }
    [Required(ErrorMessage = "El Stock es requerido")]
    public int Stock { get; set; }
    [Required(ErrorMessage = "El Id de la marca es requerido")]
    public int MarcaId { get; set; }

    [Required(ErrorMessage = "El Id de la categoría es requerido")]
    public int CategoriaId { get; set; }
    
   
    [Required(ErrorMessage = "El precio es requerido")]
    public decimal Precio { get; set; }
    [Required(ErrorMessage = "La imagen es requerido")]
    public IFormFile Foto { get; set; } // foto
    
}
