using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Business.Services
{
    public class ProductoService : IProductoService
    {
        private IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public List<Producto> GetAll()
        {
            return _repository.GetAll();
        }

        public Producto GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Producto producto)
        {
            _repository.Add(producto);
        }

        public void Update(Producto producto)
        {
            _repository.Update(producto);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
