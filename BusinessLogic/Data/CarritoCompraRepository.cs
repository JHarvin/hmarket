using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Data;

public class CarritoCompraRepository : ICarritoCompraRepository
{
    private readonly IDatabase _database;
    public CarritoCompraRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();   
    }
    public async Task<bool> deleteCarritoCompraAsync(string carritoId)
    {
      return await  _database.KeyDeleteAsync(carritoId); // por defecto keydeleteasync devuelve bool
    }

    public async Task<CarritoCompra> getCarritoCompraAsync(string carritoId)
    {

       var data = await _database.StringGetAsync(carritoId);
       return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CarritoCompra>(data);

        
    }

    public async Task<CarritoCompra> updateCarritoCompraAsync(CarritoCompra carrito)
    {
        // la funcion stringsetasync lleva trees parametros, el ultimo es el tiempo que estara disponible en memoria del servidor la lista de compras o carrito
     var status = await _database.StringSetAsync(carrito.Id, JsonSerializer.Serialize(carrito),TimeSpan.FromDays(30));
        if (!status) return null;

        return await getCarritoCompraAsync(carrito.Id);
    }
}
