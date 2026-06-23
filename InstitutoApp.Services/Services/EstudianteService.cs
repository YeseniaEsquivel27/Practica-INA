using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using InstitutoApp.Entities;
using System.Text.RegularExpressions;

namespace InstitutoApp.Services.Servicios;

public class ServicioEstudiante : IServicioEstudiante
{
    private readonly IRepositorioEstudiante _repositorio;
    private readonly IRepositorioMatricula _repositorioMatricula;

    public ServicioEstudiante(IRepositorioEstudiante repositorio, IRepositorioMatricula repositorioMatricula)
    {
        _repositorio = repositorio;
        _repositorioMatricula = repositorioMatricula;
    }

    public async Task<Respuesta<List<EstudianteResponseDTO>>> ObtenerTodosAsync()
    {
        var estudiantes = await _repositorio.ObtenerTodosAsync();
        var estudiantesRespuesta = estudiantes.Select(Mapear).ToList();

        return Respuesta<List<EstudianteResponseDTO>>.Correcta(
            estudiantesRespuesta,
            "Estudiantes consultados correctamente.");
    }

    public async Task<Respuesta<EstudianteResponseDTO>> ObtenerPorIdAsync(int id)
    {
        var estudiante = await _repositorio.ObtenerPorIdAsync(id);

        if (estudiante is null)
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta("Estudiante no encontrado.");
        }

        return Respuesta<EstudianteResponseDTO>.Correcta(
            Mapear(estudiante),
            "Estudiante consultado correctamente.");
    }

    public async Task<Respuesta<EstudianteResponseDTO>> CrearAsync(EstudianteCreateDTO dto)
    {
        var cedula = NormalizarCedula(dto.Cedula);
        var correo = NormalizarCorreo(dto.CorreoElectronico);

        if (!EsCedulaCostarricenseValida(cedula))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "La cédula debe tener el formato costarricense: 9 dígitos y un primer dígito entre 1 y 9.");
        }

        if (await _repositorio.ExisteCedulaAsync(cedula))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "Ya existe un estudiante con esa cédula.");
        }

        if (await _repositorio.ExisteCorreoAsync(correo))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "Ya existe un estudiante con ese correo electrónico.");
        }

        var ahora = DateTime.UtcNow;
        var estudiante = new Estudiante
        {
            Cedula = cedula,
            Nombre = dto.Nombre.Trim(),
            PrimerApellido = dto.PrimerApellido.Trim(),
            SegundoApellido = RecortarONulo(dto.SegundoApellido),
            CorreoElectronico = correo,
            Telefono = RecortarONulo(dto.Telefono),
            Estado = true,
            FechaCreacion = ahora,
            FechaActualizacion = ahora
        };

        await _repositorio.CrearAsync(estudiante);

        return Respuesta<EstudianteResponseDTO>.Correcta(
            Mapear(estudiante),
            "Estudiante creado correctamente.");
    }

    public async Task<Respuesta<EstudianteResponseDTO>> ActualizarAsync(int id, EstudianteUpdateDTO dto)
    {
        var estudiante = await _repositorio.ObtenerPorIdAsync(id);

        if (estudiante is null)
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta("Estudiante no encontrado.");
        }

        var cedula = NormalizarCedula(dto.Cedula);
        var correo = NormalizarCorreo(dto.CorreoElectronico);

        if (!EsCedulaCostarricenseValida(cedula))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "La cédula debe tener el formato costarricense: 9 dígitos y un primer dígito entre 1 y 9.");
        }

        var seEstaDesactivando = estudiante.Estado && !dto.Estado;

        if (seEstaDesactivando &&
            await _repositorioMatricula.TieneMatriculasActivasDeEstudianteAsync(id))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "No se puede desactivar un estudiante con matrículas activas.");
        }

        if (await _repositorio.ExisteCedulaAsync(cedula, id))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "Ya existe otro estudiante con esa cédula.");
        }

        if (await _repositorio.ExisteCorreoAsync(correo, id))
        {
            return Respuesta<EstudianteResponseDTO>.Incorrecta(
                "Ya existe otro estudiante con ese correo electrónico.");
        }

        estudiante.Cedula = cedula;
        estudiante.Nombre = dto.Nombre.Trim();
        estudiante.PrimerApellido = dto.PrimerApellido.Trim();
        estudiante.SegundoApellido = RecortarONulo(dto.SegundoApellido);
        estudiante.CorreoElectronico = correo;
        estudiante.Telefono = RecortarONulo(dto.Telefono);
        estudiante.Estado = dto.Estado;

        await _repositorio.ActualizarAsync(estudiante);

        return Respuesta<EstudianteResponseDTO>.Correcta(
            Mapear(estudiante),
            "Estudiante actualizado correctamente.");
    }

    public async Task<Respuesta<string>> EliminarAsync(int id)
    {
        if (await _repositorioMatricula.TieneMatriculasActivasDeEstudianteAsync(id))
        {
            return Respuesta<string>.Incorrecta(
                "No se puede eliminar un estudiante con matrículas activas.");
        }

        var estudianteEliminado = await _repositorio.EliminarAsync(id);

        if (!estudianteEliminado)
        {
            return Respuesta<string>.Incorrecta("Estudiante no encontrado.");
        }

        return Respuesta<string>.Correcta("", "Estudiante eliminado correctamente.");
    }

    private static string? RecortarONulo(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        return valor.Trim();
    }

    private static string NormalizarCedula(string cedula)
    {
        return cedula.Trim().Replace("-", string.Empty);
    }

    private static string NormalizarCorreo(string correo)
    {
        return correo.Trim().ToLowerInvariant();
    }

    private static bool EsCedulaCostarricenseValida(string cedula)
    {
        return Regex.IsMatch(cedula, @"^[1-9]\d{8}$");
    }

    private static EstudianteResponseDTO Mapear(Estudiante estudiante)
    {
        return new EstudianteResponseDTO
        {
            Id = estudiante.Id,
            Cedula = estudiante.Cedula,
            NombreCompleto = ObtenerNombreCompleto(estudiante),
            CorreoElectronico = estudiante.CorreoElectronico,
            Telefono = estudiante.Telefono,
            Estado = estudiante.Estado,
            FechaCreacion = estudiante.FechaCreacion,
            FechaActualizacion = estudiante.FechaActualizacion
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
