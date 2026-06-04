using Practica_INA.Data.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Data.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private static List<Producto> lista = new List<Producto>()
        {
            new Producto { Id = 1, Nombre = "Laptop", Descripcion = "Laptop gamer", Precio = 1500, Stock = 10 },
            new Producto { Id = 2, Nombre = "Mouse", Descripcion = "Mouse inalambrico", Precio = 25, Stock = 50 }
        };

        public List<Producto> GetAll()
        {
            return lista;
        }

        public Producto GetById(int id)
        {
            return lista.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Producto producto)
        {
            producto.Id = lista.Count + 1;
            lista.Add(producto);
        }

        public void Update(Producto producto)
        {
            var prod = lista.FirstOrDefault(p => p.Id == producto.Id);
            if (prod != null)
            {
                prod.Nombre = producto.Nombre;
                prod.Descripcion = producto.Descripcion;
                prod.Precio = producto.Precio;
                prod.Stock = producto.Stock;
            }
        }

        public void Delete(int id)
        {
            var prod = lista.FirstOrDefault(p => p.Id == id);
            if (prod != null)
            {
                lista.Remove(prod);
            }
        }
    }
}
