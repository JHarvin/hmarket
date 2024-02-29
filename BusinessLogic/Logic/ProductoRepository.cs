using BusinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly MarketDbContext _context;
        public ProductoRepository(MarketDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Producto>> GetAllProducto()
        {
            return await _context.Producto.Include(p => p.Marca).Include(p => p.Categoria).ToListAsync();
        }

        public async Task<Producto> GetProductoById(int id)
        {
            return await _context.Producto.Include(p => p.Marca).Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id==id);
        }
    }
}
