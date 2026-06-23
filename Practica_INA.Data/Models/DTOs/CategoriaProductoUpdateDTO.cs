namespace Practica_INA.Data.Models.DTOs
{
    public class CategoriaProductoUpdateDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}
