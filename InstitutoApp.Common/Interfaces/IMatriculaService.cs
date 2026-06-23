using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;

namespace InstitutoApp.Common.Interfaces;

public interface IServicioMatricula
{
    Task<Respuesta<List<MatriculaResponseDTO>>> ObtenerTodosAsync();
    Task<Respuesta<MatriculaResponseDTO>> ObtenerPorIdAsync(int id);
    Task<Respuesta<MatriculaResponseDTO>> CrearAsync(MatriculaCreateDTO dto);
}
