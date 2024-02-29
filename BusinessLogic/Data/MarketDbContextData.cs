using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Data
{
    public class MarketDbContextData
    {
        public static async Task CargarDataAsync(MarketDbContext dbcontext, ILoggerFactory logger)
        {
            try
            {
                if (!dbcontext.Marca.Any()) {
                    var marcaData = File.ReadAllText("../BusinessLogic/CargarData/marca.json");
                    var marcas = JsonSerializer.Deserialize<List<Marca>>(marcaData);
                    foreach (var marca in marcas)
                    {
                        dbcontext.Marca.Add(marca);
                    }
                    await dbcontext.SaveChangesAsync();
                }

                if (!dbcontext.Categoria.Any())
                {
                    var categoriaData = File.ReadAllText("../BusinessLogic/CargarData/categoria.json");
                    var categorias = JsonSerializer.Deserialize<List<Categoria>>(categoriaData);
                    foreach (var categoria in categorias)
                    {
                        dbcontext.Categoria.Add(categoria);
                    }
                    await dbcontext.SaveChangesAsync();
                }

                if (!dbcontext.Producto.Any())
                {
                    var productoData = File.ReadAllText("../BusinessLogic/CargarData/producto.json");
                    var productos = JsonSerializer.Deserialize<List<Producto>>(productoData);
                    foreach (var producto in productos)
                    {
                        dbcontext.Producto.Add(producto);
                    }
                    await dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var log = logger.CreateLogger<MarketDbContextData>();
                log.LogError("Error insertando en la bd: ",ex);
            }
        }
    }
}
