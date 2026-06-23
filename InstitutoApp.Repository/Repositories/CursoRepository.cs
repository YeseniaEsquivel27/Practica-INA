using InstitutoApp.Common.Interfaces;
using InstitutoApp.Data.Context;
using InstitutoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstitutoApp.Repository.Repositorios;

public class RepositorioCurso : IRepositorioCurso
{
    private readonly ApplicationDbContext _contexto;

    public RepositorioCurso(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Curso>> ObtenerTodosAsync()
    {
        return await _contexto.Cursos
            .AsNoTracking()
            .Where(curso => curso.Estado)
            .OrderBy(curso => curso.Nombre)
            .ToListAsync();
    }

    public async Task<Curso?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Cursos
            .AsNoTracking()
            .FirstOrDefaultAsync(curso => curso.Id == id && curso.Estado);
    }

    public async Task CrearAsync(Curso curso)
    {
        _contexto.Cursos.Add(curso);

        await _contexto.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Curso curso)
    {
        curso.FechaActualizacion = DateTime.UtcNow;

        _contexto.Cursos.Update(curso);

        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var curso = await _contexto.Cursos
            .FirstOrDefaultAsync(curso => curso.Id == id && curso.Estado);

        if (curso is null)
        {
            return false;
        }

        curso.Estado = false;
        curso.FechaActualizacion = DateTime.UtcNow;

        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null)
    {
        var consulta = _contexto.Cursos
            .Where(curso => curso.Nombre.ToUpper() == nombre.ToUpper());

        if (excluirId.HasValue)
        {
            consulta = consulta.Where(curso => curso.Id != excluirId.Value);
        }

        return await consulta.AnyAsync();
    }
}
