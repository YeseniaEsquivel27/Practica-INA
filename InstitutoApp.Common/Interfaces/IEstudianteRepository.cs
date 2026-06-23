using InstitutoApp.Entities;

namespace InstitutoApp.Common.Interfaces;

public interface IRepositorioEstudiante
{
    Task<List<Estudiante>> ObtenerTodosAsync();
    Task<Estudiante?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Estudiante entity);
    Task ActualizarAsync(Estudiante entity);
    Task<bool> EliminarAsync(int id);
    Task<bool> ExisteCedulaAsync(string cedula, int? excluirId = null);
    Task<bool> ExisteCorreoAsync(string correo, int? excluirId = null);
}
