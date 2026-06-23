using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Business.Interfaces
{
    public interface ICategoriaProductoService
    {
        Task<Response<List<CategoriaProductoResponseDTO>>> ObtenerTodosAsync();
        Task<Response<CategoriaProductoResponseDTO>> ObtenerPorIdAsync(int id);
        Task<Response<CategoriaProductoResponseDTO>> CrearAsync(CategoriaProductoCreateDTO dto);
        Task<Response<CategoriaProductoResponseDTO>> ActualizarAsync(CategoriaProductoUpdateDTO dto);
        Task<Response<string>> EliminarAsync(int id);
    }
}
