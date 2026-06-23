using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InstitutoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private const string RutaEstudiantes = "/api/estudiantes";

    private readonly IServicioEstudiante _servicio;

    public EstudiantesController(IServicioEstudiante servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<List<EstudianteResponseDTO>>>> ObtenerTodos()
    {
        var respuesta = await _servicio.ObtenerTodosAsync();

        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Respuesta<EstudianteResponseDTO>>> ObtenerPorId(int id)
    {
        var respuesta = await _servicio.ObtenerPorIdAsync(id);

        if (!respuesta.EsExitosa)
        {
            return NotFound(respuesta);
        }

        return Ok(respuesta);
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<EstudianteResponseDTO>>> Crear(EstudianteCreateDTO dto)
    {
        var respuesta = await _servicio.CrearAsync(dto);

        if (!respuesta.EsExitosa)
        {
            return BadRequest(respuesta);
        }

        var estudianteCreado = respuesta.Datos;

        if (estudianteCreado is null)
        {
            return BadRequest(respuesta);
        }

        var ubicacion = $"{RutaEstudiantes}/{estudianteCreado.Id}";

        return Created(ubicacion, respuesta);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Respuesta<EstudianteResponseDTO>>> Actualizar(int id, EstudianteUpdateDTO dto)
    {
        var respuesta = await _servicio.ActualizarAsync(id, dto);

        if (!respuesta.EsExitosa)
        {
            return BadRequest(respuesta);
        }

        return Ok(respuesta);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Respuesta<string>>> Eliminar(int id)
    {
        var respuesta = await _servicio.EliminarAsync(id);

        if (!respuesta.EsExitosa)
        {
            return NotFound(respuesta);
        }

        return Ok(respuesta);
    }
}
