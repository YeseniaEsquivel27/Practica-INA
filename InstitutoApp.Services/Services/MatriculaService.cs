using InstitutoApp.Common.Enums;
using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using InstitutoApp.Entities;

namespace InstitutoApp.Services.Servicios;

public class ServicioMatricula : IServicioMatricula
{
    private const int MaxCursosActivosPorPeriodo = 4;

    private readonly IRepositorioMatricula _repositorioMatricula;
    private readonly IRepositorioEstudiante _repositorioEstudiante;
    private readonly IRepositorioCurso _repositorioCurso;

    public ServicioMatricula(IRepositorioMatricula repositorioMatricula, IRepositorioEstudiante repositorioEstudiante, IRepositorioCurso repositorioCurso)
    {
        _repositorioMatricula = repositorioMatricula;
        _repositorioEstudiante = repositorioEstudiante;
        _repositorioCurso = repositorioCurso;
    }

    public async Task<Respuesta<List<MatriculaResponseDTO>>> ObtenerTodosAsync()
    {
        var matriculas = await _repositorioMatricula.ObtenerTodosAsync();
        var matriculasRespuesta = matriculas.Select(Mapear).ToList();

        return Respuesta<List<MatriculaResponseDTO>>.Correcta(
            matriculasRespuesta,
            "Matrículas consultadas correctamente.");
    }

    public async Task<Respuesta<MatriculaResponseDTO>> ObtenerPorIdAsync(int id)
    {
        var matricula = await _repositorioMatricula.ObtenerPorIdAsync(id);

        if (matricula is null)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta("Matrícula no encontrada.");
        }

        return Respuesta<MatriculaResponseDTO>.Correcta(
            Mapear(matricula),
            "Matrícula consultada correctamente.");
    }

    public async Task<Respuesta<MatriculaResponseDTO>> CrearAsync(MatriculaCreateDTO dto)
    {
        var estudiante = await _repositorioEstudiante.ObtenerPorIdAsync(dto.EstudianteId);

        if (estudiante is null)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "El estudiante no existe o se encuentra inactivo.");
        }

        var curso = await _repositorioCurso.ObtenerPorIdAsync(dto.CursoId);

        if (curso is null)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "El curso no existe o se encuentra inactivo.");
        }

        var fechaActual = DateTime.UtcNow;
        var matriculaEstaAbierta = fechaActual >= curso.FechaInicioMatricula &&
                                    fechaActual <= curso.FechaCierreMatricula;

        if (!matriculaEstaAbierta)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "El período de matrícula para este curso no está abierto.");
        }

        if (await _repositorioMatricula.ExisteActivaAsync(dto.EstudianteId, dto.CursoId))
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "El estudiante ya está matriculado en este curso.");
        }

        var cantidadMatriculasCurso = await _repositorioMatricula
            .ContarActivasPorCursoAsync(dto.CursoId);

        if (cantidadMatriculasCurso >= curso.CupoMaximo)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "No es posible matricular: el curso ya alcanzó su cupo máximo.");
        }

        var cantidadCursosEstudiante = await _repositorioMatricula
            .ContarActivasPorEstudianteYPeriodoAsync(
                dto.EstudianteId,
                curso.PeriodoAcademico);

        if (cantidadCursosEstudiante >= MaxCursosActivosPorPeriodo)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                $"El estudiante alcanzó el límite de {MaxCursosActivosPorPeriodo} cursos activos " +
                $"para el período {curso.PeriodoAcademico}.");
        }

        var matricula = new Matricula
        {
            EstudianteId = estudiante.Id,
            CursoId = curso.Id,
            PeriodoAcademico = curso.PeriodoAcademico,
            FechaMatricula = fechaActual,
            Estado = true,
            FechaActualizacion = fechaActual
        };

        var resultado = await _repositorioMatricula.CrearConReglasAsync(
            matricula,
            curso.CupoMaximo,
            MaxCursosActivosPorPeriodo);

        var mensajeError = ObtenerErrorDeCreacion(
            resultado,
            curso.PeriodoAcademico);

        if (mensajeError is not null)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(mensajeError);
        }

        var matriculaCreada = await _repositorioMatricula.ObtenerPorIdAsync(matricula.Id);

        if (matriculaCreada is null)
        {
            return Respuesta<MatriculaResponseDTO>.Incorrecta(
                "No fue posible consultar la matrícula recién creada.");
        }

        return Respuesta<MatriculaResponseDTO>.Correcta(
            Mapear(matriculaCreada),
            "Matrícula creada correctamente.");
    }

    private static string? ObtenerErrorDeCreacion(ResultadoCreacionMatricula resultado, string periodoAcademico)
    {
        switch (resultado)
        {
            case ResultadoCreacionMatricula.Creada:
                return null;

            case ResultadoCreacionMatricula.Duplicada:
                return "El estudiante ya está matriculado en este curso.";

            case ResultadoCreacionMatricula.CupoCompleto:
                return "No es posible matricular: el curso ya alcanzó su cupo máximo.";

            case ResultadoCreacionMatricula.LimiteCursosAlcanzado:
                return $"El estudiante alcanzó el límite de {MaxCursosActivosPorPeriodo} " +
                       $"cursos activos para el período {periodoAcademico}.";

            default:
                return "No fue posible crear la matrícula.";
        }
    }

    private static MatriculaResponseDTO Mapear(Matricula matricula)
    {
        return new MatriculaResponseDTO
        {
            Id = matricula.Id,
            PeriodoAcademico = matricula.PeriodoAcademico,
            FechaMatricula = matricula.FechaMatricula,
            Estado = matricula.Estado,
            FechaActualizacion = matricula.FechaActualizacion,
            Estudiante = new EstudianteResumenDTO
            {
                Id = matricula.Estudiante.Id,
                Cedula = matricula.Estudiante.Cedula,
                NombreCompleto = ObtenerNombreCompleto(matricula.Estudiante)
            },
            Curso = new CursoResumenDTO
            {
                Id = matricula.Curso.Id,
                Nombre = matricula.Curso.Nombre
            }
        };
    }

    private static string ObtenerNombreCompleto(Estudiante estudiante)
    {
        var partesNombre = new[]
        {
            estudiante.Nombre,
            estudiante.PrimerApellido,
            estudiante.SegundoApellido
        };

        return string.Join(
            " ",
            partesNombre.Where(parte => !string.IsNullOrWhiteSpace(parte)));
    }
}
