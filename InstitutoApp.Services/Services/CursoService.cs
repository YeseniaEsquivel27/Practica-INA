using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using InstitutoApp.Entities;
using System.Text.RegularExpressions;

namespace InstitutoApp.Services.Servicios;

public class ServicioCurso : IServicioCurso
{
    private readonly IRepositorioCurso _repositorio;
    private readonly IRepositorioMatricula _repositorioMatricula;

    public ServicioCurso(IRepositorioCurso repositorio, IRepositorioMatricula repositorioMatricula)
    {
        _repositorio = repositorio;
        _repositorioMatricula = repositorioMatricula;
    }

    public async Task<Respuesta<List<CursoResponseDTO>>> ObtenerTodosAsync()
    {
        var cursos = await _repositorio.ObtenerTodosAsync();
        var cursosRespuesta = cursos.Select(Mapear).ToList();

        return Respuesta<List<CursoResponseDTO>>.Correcta(
            cursosRespuesta,
            "Cursos consultados correctamente.");
    }

    public async Task<Respuesta<CursoResponseDTO>> ObtenerPorIdAsync(int id)
    {
        var curso = await _repositorio.ObtenerPorIdAsync(id);

        if (curso is null)
        {
            return Respuesta<CursoResponseDTO>.Incorrecta("Curso no encontrado.");
        }

        return Respuesta<CursoResponseDTO>.Correcta(
            Mapear(curso),
            "Curso consultado correctamente.");
    }

    public async Task<Respuesta<CursoResponseDTO>> CrearAsync(CursoCreateDTO dto)
    {
        var nombre = NormalizarNombre(dto.Nombre);
        var periodo = NormalizarPeriodo(dto.PeriodoAcademico);

        if (!FechasMatriculaSonValidas(dto))
        {
            return Respuesta<CursoResponseDTO>.Incorrecta(
                "La fecha de inicio de matrícula debe ser anterior a la fecha de cierre.");
        }

        if (await _repositorio.ExisteNombreAsync(nombre))
        {
            return Respuesta<CursoResponseDTO>.Incorrecta("Ya existe un curso con ese nombre.");
        }

        var ahora = DateTime.UtcNow;
        var curso = new Curso
        {
            Nombre = nombre,
            Descripcion = RecortarONulo(dto.Descripcion),
            CupoMaximo = dto.CupoMaximo,
            PeriodoAcademico = periodo,
            FechaInicioMatricula = dto.FechaInicioMatricula!.Value,
            FechaCierreMatricula = dto.FechaCierreMatricula!.Value,
            Estado = true,
            FechaCreacion = ahora,
            FechaActualizacion = ahora
        };

        await _repositorio.CrearAsync(curso);

        return Respuesta<CursoResponseDTO>.Correcta(
            Mapear(curso),
            "Curso creado correctamente.");
    }

    public async Task<Respuesta<CursoResponseDTO>> ActualizarAsync(int id, CursoUpdateDTO dto)
    {
        var curso = await _repositorio.ObtenerPorIdAsync(id);

        if (curso is null)
        {
            return Respuesta<CursoResponseDTO>.Incorrecta("Curso no encontrado.");
        }

        var nombre = NormalizarNombre(dto.Nombre);
        var periodo = NormalizarPeriodo(dto.PeriodoAcademico);

        if (!FechasMatriculaSonValidas(dto))
        {
            return Respuesta<CursoResponseDTO>.Incorrecta(
                "La fecha de inicio de matrícula debe ser anterior a la fecha de cierre.");
        }

        if (await _repositorio.ExisteNombreAsync(nombre, id))
        {
            return Respuesta<CursoResponseDTO>.Incorrecta("Ya existe otro curso con ese nombre.");
        }

        var matriculasActivas = await _repositorioMatricula.ContarActivasPorCursoAsync(id);

        if (dto.CupoMaximo < matriculasActivas)
        {
            return Respuesta<CursoResponseDTO>.Incorrecta(
                "El cupo máximo no puede ser menor que la cantidad actual de estudiantes matriculados.");
        }

        if (curso.PeriodoAcademico != periodo && matriculasActivas > 0)
        {
            return Respuesta<CursoResponseDTO>.Incorrecta(
                "No se puede cambiar el período de un curso con matrículas activas.");
        }

        var seEstaDesactivando = curso.Estado && !dto.Estado;

        if (seEstaDesactivando &&
            await _repositorioMatricula.TieneMatriculasActivasDeCursoAsync(id))
        {
            return Respuesta<CursoResponseDTO>.Incorrecta(
                "No se puede desactivar un curso con matrículas activas.");
        }

        curso.Nombre = nombre;
        curso.Descripcion = RecortarONulo(dto.Descripcion);
        curso.CupoMaximo = dto.CupoMaximo;
        curso.PeriodoAcademico = periodo;
        curso.FechaInicioMatricula = dto.FechaInicioMatricula!.Value;
        curso.FechaCierreMatricula = dto.FechaCierreMatricula!.Value;
        curso.Estado = dto.Estado;

        await _repositorio.ActualizarAsync(curso);

        return Respuesta<CursoResponseDTO>.Correcta(
            Mapear(curso),
            "Curso actualizado correctamente.");
    }

    public async Task<Respuesta<string>> EliminarAsync(int id)
    {
        if (await _repositorioMatricula.TieneMatriculasActivasDeCursoAsync(id))
        {
            return Respuesta<string>.Incorrecta(
                "No se puede eliminar un curso con matrículas activas.");
        }

        var cursoEliminado = await _repositorio.EliminarAsync(id);

        if (!cursoEliminado)
        {
            return Respuesta<string>.Incorrecta("Curso no encontrado.");
        }

        return Respuesta<string>.Correcta("", "Curso eliminado correctamente.");
    }

    private static bool FechasMatriculaSonValidas(CursoCreateDTO dto)
    {
        if (!dto.FechaInicioMatricula.HasValue || !dto.FechaCierreMatricula.HasValue)
        {
            return false;
        }

        return dto.FechaInicioMatricula <= dto.FechaCierreMatricula;
    }

    private static string? RecortarONulo(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        return valor.Trim();
    }

    private static string NormalizarNombre(string nombre)
    {
        return Regex.Replace(nombre.Trim(), @"\s+", " ");
    }

    private static string NormalizarPeriodo(string periodo)
    {
        return periodo.Trim().ToUpperInvariant();
    }

    private static CursoResponseDTO Mapear(Curso curso)
    {
        return new CursoResponseDTO
        {
            Id = curso.Id,
            Nombre = curso.Nombre,
            Descripcion = curso.Descripcion,
            CupoMaximo = curso.CupoMaximo,
            PeriodoAcademico = curso.PeriodoAcademico,
            FechaInicioMatricula = curso.FechaInicioMatricula,
            FechaCierreMatricula = curso.FechaCierreMatricula,
            Estado = curso.Estado,
            FechaCreacion = curso.FechaCreacion,
            FechaActualizacion = curso.FechaActualizacion
        };
    }
}
