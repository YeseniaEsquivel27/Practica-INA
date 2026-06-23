using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Business.Interfaces
{
    public interface IProductoService
    {
        Task<Response<List<ProductoResponseDTO>>> ObtenerTodosAsync();
        Task<Response<ProductoResponseDTO>> ObtenerPorIdAsync(int id);
        Task<Response<ProductoResponseDTO>> CrearAsync(ProductoCreateDTO dto);
        Task<Response<ProductoResponseDTO>> ActualizarAsync(ProductoUpdateDTO dto);
        Task<Response<string>> EliminarAsync(int id);
    }
}
