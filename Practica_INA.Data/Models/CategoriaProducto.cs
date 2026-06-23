namespace Practica_INA.Data.Models
{
    public class CategoriaProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relación 1:N con Producto
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
