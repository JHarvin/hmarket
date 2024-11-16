using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers;


public class CarritoCompraController : BaseApiController
{
    private readonly ICarritoCompraRepository _compraRepository;
    public CarritoCompraController(ICarritoCompraRepository carritoCompra)
    {
        _compraRepository = carritoCompra;
    }

    [HttpGet("GetCarritoById")]
    public async Task<ActionResult<CarritoCompra>> GetCarritoById(string id) {

         var carrito = await _compraRepository.getCarritoCompraAsync(id);
        return Ok(carrito ?? new CarritoCompra(id));
    
    }
    //sirve para guardar y actualizar
    [HttpPost("updateCarritoCompra")]
    public async Task<ActionResult<CarritoCompra>> updateCarritoCompra(CarritoCompra carritoParam)
    {
      var carritoActualizado = await _compraRepository.updateCarritoCompraAsync(carritoParam);
      
     return Ok(carritoActualizado);
    }

    [HttpDelete("deleteCarritoCompra")]
    public async Task deleteCarritoCompra(string id) {

       await _compraRepository.deleteCarritoCompraAsync(id);
    }


    }
