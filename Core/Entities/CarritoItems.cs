﻿namespace Core.Entities;

public class CarritoItems
{
    public int Id { get; set; }
    public string Producto { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public string Imagen { get; set; }
    public string Marca { get; set; }
    public string Categoria { get; set; }
}
