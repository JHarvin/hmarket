using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface ICarritoCompraRepository
{
    Task<CarritoCompra> getCarritoCompraAsync(string carritoId);
    Task<CarritoCompra> updateCarritoCompraAsync(CarritoCompra carrito);
    Task<bool> deleteCarritoCompraAsync(string carritoId);
}
