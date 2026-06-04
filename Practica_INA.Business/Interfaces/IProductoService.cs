using Practica_INA.Data.Models;

namespace Practica_INA.Business.Interfaces
{
    public interface IProductoService
    {
        List<Producto> GetAll();
        Producto GetById(int id);
        void Add(Producto producto);
        void Update(Producto producto);
        void Delete(int id);
    }
}
