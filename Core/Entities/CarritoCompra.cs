using System.Collections.Generic;

namespace Core.Entities;

public class CarritoCompra
{
    public CarritoCompra(string id)
    {
        Id = id;
    }
    public CarritoCompra()
    {
        
    }
    public string Id { get; set; }
    public List<CarritoItems> Items { get; set; } = new List<CarritoItems>(); 
}
