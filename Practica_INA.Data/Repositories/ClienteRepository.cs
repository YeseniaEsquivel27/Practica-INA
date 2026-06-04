using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private static List<Cliente> lista = new List<Cliente>()
        {
            new Cliente { Id = 1, Nombre = "Juan", Apellido = "Perez", Email = "juan@mail.com", Telefono = "1234567890" },
            new Cliente { Id = 2, Nombre = "Maria", Apellido = "Lopez", Email = "maria@mail.com", Telefono = "0987654321" }
        };

        public List<Cliente> GetAll()
        {
            return lista;
        }

        public Cliente GetById(int id)
        {
            return lista.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Cliente cliente)
        {
            cliente.Id = lista.Count + 1;
            lista.Add(cliente);
        }

        public void Update(Cliente cliente)
        {
            var cli = lista.FirstOrDefault(c => c.Id == cliente.Id);
            if (cli != null)
            {
                cli.Nombre = cliente.Nombre;
                cli.Apellido = cliente.Apellido;
                cli.Email = cliente.Email;
                cli.Telefono = cliente.Telefono;
            }
        }

        public void Delete(int id)
        {
            var cli = lista.FirstOrDefault(c => c.Id == id);
            if (cli != null)
            {
                lista.Remove(cli);
            }
        }
    }
}
