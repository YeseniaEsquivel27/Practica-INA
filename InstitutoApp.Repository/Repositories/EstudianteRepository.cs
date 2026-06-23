using InstitutoApp.Common.Interfaces;
using InstitutoApp.Data.Context;
using InstitutoApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstitutoApp.Repository.Repositorios;

public class RepositorioEstudiante : IRepositorioEstudiante
{
    private readonly ApplicationDbContext _contexto;

    public RepositorioEstudiante(ApplicationDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Estudiante>> ObtenerTodosAsync()
    {
        return await _contexto.Estudiantes
            .AsNoTracking()
            .Where(estudiante => estudiante.Estado)
            .OrderBy(estudiante => estudiante.PrimerApellido)
            .ToListAsync();
    }

    public async Task<Estudiante?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(estudiante => estudiante.Id == id && estudiante.Estado);
    }

    public async Task CrearAsync(Estudiante estudiante)
    {
        _contexto.Estudiantes.Add(estudiante);

        await _contexto.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Estudiante estudiante)
    {
        estudiante.FechaActualizacion = DateTime.UtcNow;

        _contexto.Estudiantes.Update(estudiante);

        await _contexto.SaveChangesAsync();
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var estudiante = await _contexto.Estudiantes
            .FirstOrDefaultAsync(estudiante => estudiante.Id == id && estudiante.Estado);

        if (estudiante is null)
        {
            return false;
        }

        estudiante.Estado = false;
        estudiante.FechaActualizacion = DateTime.UtcNow;

        await _contexto.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExisteCedulaAsync(string cedula, int? excluirId = null)
    {
        var consulta = _contexto.Estudiantes
            .Where(estudiante => estudiante.Cedula == cedula);

        if (excluirId.HasValue)
        {
            consulta = consulta.Where(estudiante => estudiante.Id != excluirId.Value);
        }

        return await consulta.AnyAsync();
    }

    public async Task<bool> ExisteCorreoAsync(string correo, int? excluirId = null)
    {
        var consulta = _contexto.Estudiantes
            .Where(estudiante => estudiante.CorreoElectronico == correo);

        if (excluirId.HasValue)
        {
            consulta = consulta.Where(estudiante => estudiante.Id != excluirId.Value);
        }

        return await consulta.AnyAsync();
    }
}
