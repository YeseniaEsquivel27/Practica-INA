using Practica_INA.Data.Models;

namespace Practica_INA.Data.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<Producto> ObtenerPorNombreAsync(string nombre);
    }
}
