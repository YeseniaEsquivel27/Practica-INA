using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;

namespace InstitutoApp.Common.Interfaces;

public interface IServicioEstudiante
{
    Task<Respuesta<List<EstudianteResponseDTO>>> ObtenerTodosAsync();
    Task<Respuesta<EstudianteResponseDTO>> ObtenerPorIdAsync(int id);
    Task<Respuesta<EstudianteResponseDTO>> CrearAsync(EstudianteCreateDTO dto);
    Task<Respuesta<EstudianteResponseDTO>> ActualizarAsync(int id, EstudianteUpdateDTO dto);
    Task<Respuesta<string>> EliminarAsync(int id);
}
