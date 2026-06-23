using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;

namespace InstitutoApp.Common.Interfaces;

public interface IServicioCurso
{
    Task<Respuesta<List<CursoResponseDTO>>> ObtenerTodosAsync();
    Task<Respuesta<CursoResponseDTO>> ObtenerPorIdAsync(int id);
    Task<Respuesta<CursoResponseDTO>> CrearAsync(CursoCreateDTO dto);
    Task<Respuesta<CursoResponseDTO>> ActualizarAsync(int id, CursoUpdateDTO dto);
    Task<Respuesta<string>> EliminarAsync(int id);
}
