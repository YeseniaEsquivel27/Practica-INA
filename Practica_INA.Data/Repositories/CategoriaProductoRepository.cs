using Microsoft.EntityFrameworkCore;
using Practica_INA.Data.Context;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Data.Repositories
{
    public class CategoriaProductoRepository : ICategoriaProductoRepository
    {
        private readonly PracticaContext _context;

        public CategoriaProductoRepository(PracticaContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaProducto>> ObtenerTodosAsync()
        {
            return await _context.CategoriasProducto.AsNoTracking().ToListAsync();
        }

        public async Task<CategoriaProducto> ObtenerPorIdAsync(int id)
        {
            return await _context.CategoriasProducto.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AgregarAsync(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Update(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var categoria = await _context.CategoriasProducto.FindAsync(id);
            if (categoria != null)
            {
                _context.CategoriasProducto.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.CategoriasProducto.AnyAsync(c => c.Id == id);
        }

        public async Task<CategoriaProducto> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.CategoriasProducto
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Nombre == nombre);
        }
    }
}
