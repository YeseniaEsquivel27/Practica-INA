using InstitutoApp.Entities;
using InstitutoApp.Common.Enums;

namespace InstitutoApp.Common.Interfaces;

public interface IRepositorioMatricula
{
    Task<List<Matricula>> ObtenerTodosAsync();
    Task<Matricula?> ObtenerPorIdAsync(int id);
    Task CrearAsync(Matricula entity);
    Task<ResultadoCreacionMatricula> CrearConReglasAsync(Matricula entity, int cupoMaximo, int limiteCursosPorPeriodo);
    Task ActualizarAsync(Matricula entity);
    Task<bool> EliminarAsync(int id);
    Task<bool> ExisteActivaAsync(int estudianteId, int cursoId);
    Task<int> ContarActivasPorCursoAsync(int cursoId);
    Task<int> ContarActivasPorEstudianteYPeriodoAsync(int estudianteId, string periodoAcademico);
    Task<bool> TieneMatriculasActivasDeEstudianteAsync(int estudianteId);
    Task<bool> TieneMatriculasActivasDeCursoAsync(int cursoId);
}
