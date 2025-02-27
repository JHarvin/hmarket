﻿using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos;

public class ProductoShowDto
{
    
    public int Id { get; set; }
   
    public string Nombre { get; set; }
    
    public string Descripcion { get; set; }
    
    public int Stock { get; set; }
    
    public int MarcaId { get; set; }

   
    public int CategoriaId { get; set; }

    
    public decimal Precio { get; set; }
    public string Imagen { get; set; }
    public string CategoriaNombre { get; set; }
    public string MarcaNombre { get; set; }
}
