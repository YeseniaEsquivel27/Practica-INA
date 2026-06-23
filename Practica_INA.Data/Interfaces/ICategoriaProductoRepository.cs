using Practica_INA.Data.Models;

namespace Practica_INA.Data.Interfaces
{
    public interface ICategoriaProductoRepository
    {
        Task<List<CategoriaProducto>> ObtenerTodosAsync();
        Task<CategoriaProducto> ObtenerPorIdAsync(int id);
        Task AgregarAsync(CategoriaProducto categoria);
        Task ActualizarAsync(CategoriaProducto categoria);
        Task EliminarAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<CategoriaProducto> ObtenerPorNombreAsync(string nombre);
    }
}
