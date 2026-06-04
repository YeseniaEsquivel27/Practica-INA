using Practica_INA.Data.Models;

namespace Practica_INA.Data.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> GetAll();
        Cliente GetById(int id);
        void Add(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(int id);
    }
}
