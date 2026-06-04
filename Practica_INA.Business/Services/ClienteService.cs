using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Business.Services
{
    public class ClienteService : IClienteService
    {
        private IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public List<Cliente> GetAll()
        {
            return _repository.GetAll();
        }

        public Cliente GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Cliente cliente)
        {
            _repository.Add(cliente);
        }

        public void Update(Cliente cliente)
        {
            _repository.Update(cliente);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
