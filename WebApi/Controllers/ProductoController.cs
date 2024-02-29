using AutoMapper;
using Core.Entities;
using Core.Entities.Specifications;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers;



public class ProductoController : BaseApiController
{
    
    private readonly IGenericRepository<Producto> _productoRepository;
    private readonly IMapper _mapper;
    public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    [HttpGet(Name = "Productos")]
    public async Task<ActionResult<List<Producto>>> GetProductos(string ordenar)
    {
        var spec = new ProductoWithCategoriaAndMarca(ordenar);
        var productos = await _productoRepository.GetAllWithSpec(spec);
        return Ok(_mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> GetProducto(int id)
    {
        var spec = new ProductoWithCategoriaAndMarca(id);

      var producto= await _productoRepository.GetByIdWithSpec(spec);

        if (producto==null) {
        return NotFound(new CodeErrorResponse(404,"El producto no existe"));
        }

       return  _mapper.Map<Producto, ProductoDto>(producto);

       
    }

}
