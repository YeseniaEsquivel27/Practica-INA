namespace Practica_INA.Data.Models.DTOs
{
    public class ProductoCreateDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaProductoId { get; set; }
    }
}
