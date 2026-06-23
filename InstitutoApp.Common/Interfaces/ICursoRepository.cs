using InstitutoApp.Entities;

namespace InstitutoApp.Common.Interfaces;

public interface IRepositorioCurso
{
    Task<List<Curso>> ObtenerTodosAsync();
    Task<Curso?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Curso entity);
    Task ActualizarAsync(Curso entity);
    Task<bool> EliminarAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
}
