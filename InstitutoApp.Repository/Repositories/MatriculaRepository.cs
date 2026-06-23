using InstitutoApp.Common.Enums;
using InstitutoApp.Common.Interfaces;
using InstitutoApp.Data.Context;
using InstitutoApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InstitutoApp.Repository.Repositorios;

public class RepositorioMatricula : IRepositorioMatricula
{
    private readonly ApplicationDbContext _contexto;

    public RepositorioMatricula(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Matricula>> ObtenerTodosAsync()
    {
        return await ConsultaConRelaciones()
            .Where(matricula => matricula.Estado)
            .OrderByDescending(matricula => matricula.FechaMatricula)
            .ToListAsync();
    }

    public async Task<Matricula?> ObtenerPorIdAsync(int id)
    {
        return await ConsultaConRelaciones()
            .FirstOrDefaultAsync(matricula => matricula.Id == id && matricula.Estado);
    }

    public async Task CrearAsync(Matricula matricula)
    {
        _contexto.Matriculas.Add(matricula);

        await _contexto.SaveChangesAsync();
    }

    public async Task<ResultadoCreacionMatricula> CrearConReglasAsync(Matricula matricula, int cupoMaximo, int limiteCursosPorPeriodo)
    {
        await using var transaccion = await _contexto.Database.BeginTransactionAsync(
            IsolationLevel.Serializable);

        var matriculaDuplicada = await _contexto.Matriculas.AnyAsync(item =>
            item.EstudianteId == matricula.EstudianteId &&
            item.CursoId == matricula.CursoId &&
            item.Estado);

        if (matriculaDuplicada)
        {
            return ResultadoCreacionMatricula.Duplicada;
        }

        var cupoCompleto = await _contexto.Matriculas.CountAsync(item =>
            item.CursoId == matricula.CursoId &&
            item.Estado) >= cupoMaximo;

        if (cupoCompleto)
        {
            return ResultadoCreacionMatricula.CupoCompleto;
        }

        var limiteCursosAlcanzado = await _contexto.Matriculas.CountAsync(item =>
            item.EstudianteId == matricula.EstudianteId &&
            item.PeriodoAcademico == matricula.PeriodoAcademico &&
            item.Estado) >= limiteCursosPorPeriodo;

        if (limiteCursosAlcanzado)
        {
            return ResultadoCreacionMatricula.LimiteCursosAlcanzado;
        }

        try
        {
            _contexto.Matriculas.Add(matricula);

            await _contexto.SaveChangesAsync();
            await transaccion.CommitAsync();

            return ResultadoCreacionMatricula.Creada;
        }
        catch (DbUpdateException)
        {
            await transaccion.RollbackAsync();

            return ResultadoCreacionMatricula.Duplicada;
        }
    }

    public async Task ActualizarAsync(Matricula matricula)
    {
        matricula.FechaActualizacion = DateTime.UtcNow;

        _contexto.Matriculas.Update(matricula);

        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var matricula = await _contexto.Matriculas
            .FirstOrDefaultAsync(item => item.Id == id && item.Estado);

        if (matricula is null)
        {
            return false;
        }

        matricula.Estado = false;
        matricula.FechaActualizacion = DateTime.UtcNow;

        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExisteActivaAsync(int estudianteId, int cursoId)
    {
        return await _contexto.Matriculas.AnyAsync(matricula =>
            matricula.EstudianteId == estudianteId &&
            matricula.CursoId == cursoId &&
            matricula.Estado);
    }

    public async Task<int> ContarActivasPorCursoAsync(int cursoId)
    {
        return await _contexto.Matriculas.CountAsync(matricula =>
            matricula.CursoId == cursoId &&
            matricula.Estado);
    }

    public async Task<int> ContarActivasPorEstudianteYPeriodoAsync(int estudianteId, string periodoAcademico)
    {
        return await _contexto.Matriculas.CountAsync(matricula =>
            matricula.EstudianteId == estudianteId &&
            matricula.PeriodoAcademico == periodoAcademico &&
            matricula.Estado);
    }

    public async Task<bool> TieneMatriculasActivasDeEstudianteAsync(int estudianteId)
    {
        return await _contexto.Matriculas.AnyAsync(matricula =>
            matricula.EstudianteId == estudianteId &&
            matricula.Estado);
    }

    public async Task<bool> TieneMatriculasActivasDeCursoAsync(int cursoId)
    {
        return await _contexto.Matriculas.AnyAsync(matricula =>
            matricula.CursoId == cursoId &&
            matricula.Estado);
    }

    private IQueryable<Matricula> ConsultaConRelaciones()
    {
        return _contexto.Matriculas
            .Include(matricula => matricula.Estudiante)
            .Include(matricula => matricula.Curso)
            .AsNoTracking();
    }
}
