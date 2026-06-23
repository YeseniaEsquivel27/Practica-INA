using Microsoft.EntityFrameworkCore;
using Practica_INA.Data.Context;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Data.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly PracticaContext _context;

        public ProductoRepository(PracticaContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .Include(p => p.CategoriaProducto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.CategoriaProducto)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AgregarAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Productos.AnyAsync(p => p.Id == id);
        }

        public async Task<Producto> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Nombre == nombre);
        }
    }
}

