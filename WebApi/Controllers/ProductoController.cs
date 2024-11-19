using AutoMapper;
using Core.Entities;
using Core.Entities.Specifications;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public async Task<ActionResult<Pagination<ProductoDto>>> GetProductos([FromQuery]ProductoSpecificationParam param)
    {
        var spec = new ProductoWithCategoriaAndMarca(param);
        var productos = await _productoRepository.GetAllWithSpec(spec);
        var specCount = new ProductoForCountingSpecification(param);
        var totalProductos = await _productoRepository.CountAsync(specCount);
        var rounded = Math.Ceiling( Convert.ToDecimal( totalProductos / param.PageSize));
        var totalPage = Convert.ToInt32(rounded);
        var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos);
        return Ok(
            new Pagination<ProductoDto>
            {
                Count = totalProductos,
                Items = data,
                PageCount = totalPage,
                PageIndex = param.pageIndex,
                PageSize = param.PageSize
            }
            );
        //return Ok(_mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos));
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

    [HttpPost("addproducto")]
    public async Task<ActionResult<Producto>> AddProducto([FromBody] Producto producto ) { 
    
      var resultado = await _productoRepository.Add(producto);
        if (resultado == 0)
        {
            throw new Exception("No se inserto el producto");
        }
        return Ok(producto);
    
    }
    [HttpPut("updateproducto/{id}")]
    public async Task<ActionResult<Producto>> UpdateProducto(int id, [FromBody] Producto producto ) { 
      producto.Id = id;
        var resultado = await _productoRepository.Update(producto);  
        if (resultado == 0)
        {
            throw new Exception("No se pudo actualizar el producto");

        }
        return Ok(producto);

    
    }


}
