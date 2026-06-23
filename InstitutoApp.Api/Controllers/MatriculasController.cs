using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InstitutoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatriculasController : ControllerBase
{
    private const string RutaMatriculas = "/api/matriculas";

    private readonly IServicioMatricula _servicio;

    public MatriculasController(IServicioMatricula servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<List<MatriculaResponseDTO>>>> ObtenerTodos()
    {
        var respuesta = await _servicio.ObtenerTodosAsync();

        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Respuesta<MatriculaResponseDTO>>> ObtenerPorId(int id)
    {
        var respuesta = await _servicio.ObtenerPorIdAsync(id);

        if (!respuesta.EsExitosa)
        {
            return NotFound(respuesta);
        }

        return Ok(respuesta);
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<MatriculaResponseDTO>>> Crear(MatriculaCreateDTO dto)
    {
        var respuesta = await _servicio.CrearAsync(dto);

        if (!respuesta.EsExitosa)
        {
            return BadRequest(respuesta);
        }

        var matriculaCreada = respuesta.Datos;

        if (matriculaCreada is null)
        {
            return BadRequest(respuesta);
        }

        var ubicacion = $"{RutaMatriculas}/{matriculaCreada.Id}";

        return Created(ubicacion, respuesta);
    }
}
