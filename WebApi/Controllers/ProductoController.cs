using AutoMapper;
using Core.Entities;
using Core.Entities.Specifications;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers;



public class ProductoController : BaseApiController
{
    
    private readonly IGenericRepository<Producto> _productoRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostingEnvironment;
    public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper, IWebHostEnvironment hostingEnvironment)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;

    }

    [HttpGet(Name = "Productos")]
    public async Task<ActionResult<Pagination<ProductoShowDto>>> GetProductos([FromQuery]ProductoSpecificationParam param)
    {
        var spec = new ProductoWithCategoriaAndMarca(param);
        var productos = await _productoRepository.GetAllWithSpec(spec);
        var specCount = new ProductoForCountingSpecification(param);
        var totalProductos = await _productoRepository.CountAsync(specCount);
        var rounded = Math.Ceiling( Convert.ToDecimal( totalProductos / param.PageSize));
        var totalPage = Convert.ToInt32(rounded);
        var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoShowDto>>(productos);
        return Ok(
            new Pagination<ProductoShowDto>
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
    public async Task<ActionResult<ProductoShowDto>> GetProducto(int id)
    {
        var spec = new ProductoWithCategoriaAndMarca(id);

      var producto= await _productoRepository.GetByIdWithSpec(spec);

        if (producto==null) {
        return NotFound(new CodeErrorResponse(404,"El producto no existe"));
        }

       return  _mapper.Map<Producto, ProductoShowDto>(producto);

       
    }

    [HttpPost("addproducto")]
    public async Task<ActionResult<Producto>> AddProducto([FromForm] ProductoDto producto ) {

        //Guardar la imagen
        /* subida de imagen*/
        var nuevoProducto = new Producto();
        var archivo = producto.Foto;
        string rutaPrincipal = _hostingEnvironment.WebRootPath;
        var archivos = HttpContext.Request.Form.Files;
        if (archivo.Length > 0)
        {
            // nueva imagen
            var nombreFoto = Guid.NewGuid().ToString();
            var subidas = Path.Combine(rutaPrincipal, @"fotos");
            var extension = Path.GetExtension(archivos[0].FileName);

            using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
            {
                archivos[0].CopyTo(fileStreams);

            }
            nuevoProducto = _mapper.Map<Producto>(producto);
            nuevoProducto.Imagen = @"\fotos\" + nombreFoto + extension;
             

        }

        // fin guardar la imagen

        // agregar producto ya con la direccion de la imagen

        var resultado = await _productoRepository.Add(nuevoProducto);
        if (resultado == 0)
        {
            throw new Exception("No se inserto el producto");
        }
        return Ok(producto);
    
    }
    [HttpPut("updateproducto")]
    public async Task<ActionResult<Producto>> UpdateProducto([FromForm] ProductoUpdateDto producto ) { 
      //
      var productoExistente = await _productoRepository.GetByIdAsync(producto.Id);

        if (productoExistente == null)
        {
            return NotFound(new { mensaje = "Producto no existe, no se puede actualizar" });
        }


        // Verificar si se subió una nueva imagen
        if (producto.Foto != null && producto.Foto.Length > 0)
        {
            string rutaPrincipal = _hostingEnvironment.WebRootPath;
            var subidas = Path.Combine(rutaPrincipal, "fotos");

            // Eliminar la imagen anterior si existe
            if (!string.IsNullOrEmpty(productoExistente.Imagen))
            {
                string rutaImagenAnterior = Path.Combine(rutaPrincipal, productoExistente.Imagen.TrimStart('\\'));
                if (System.IO.File.Exists(rutaImagenAnterior))
                {
                    System.IO.File.Delete(rutaImagenAnterior);
                }
            }

            // Guardar la nueva imagen
            var nombreFoto = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(producto.Foto.FileName);
            var rutaNuevaImagen = Path.Combine(subidas, nombreFoto + extension);

            using (var fileStream = new FileStream(rutaNuevaImagen, FileMode.Create))
            {
                await producto.Foto.CopyToAsync(fileStream);
            }

            productoExistente.Imagen = @"\fotos\" + nombreFoto + extension;
        }

        // Actualizar otros datos del producto
       productoExistente.Stock = producto.Stock;
       productoExistente.Nombre = producto.Nombre;
       productoExistente.Descripcion = producto.Descripcion;
       productoExistente.MarcaId = producto.MarcaId;
       productoExistente.CategoriaId = producto.CategoriaId;
       productoExistente.Precio = producto.Precio;
       


         // Guardar cambios en la base de datos
         var resultado = await _productoRepository.Update(productoExistente);
        if (resultado == 0)
        {
            throw new Exception("No se pudo actualizar el producto");
        }

        return Ok(productoExistente);


    }


}
