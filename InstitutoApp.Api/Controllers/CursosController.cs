using InstitutoApp.Common.Interfaces;
using InstitutoApp.Common.Respuestas;
using InstitutoApp.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InstitutoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private const string RutaCursos = "/api/cursos";

    private readonly IServicioCurso _servicio;

    public CursosController(IServicioCurso servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<Respuesta<List<CursoResponseDTO>>>> ObtenerTodos()
    {
        var respuesta = await _servicio.ObtenerTodosAsync();

        return Ok(respuesta);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Respuesta<CursoResponseDTO>>> ObtenerPorId(int id)
    {
        var respuesta = await _servicio.ObtenerPorIdAsync(id);

        if (!respuesta.EsExitosa)
        {
            return NotFound(respuesta);
        }

        return Ok(respuesta);
    }

    [HttpPost]
    public async Task<ActionResult<Respuesta<CursoResponseDTO>>> Crear(CursoCreateDTO dto)
    {
        var respuesta = await _servicio.CrearAsync(dto);

        if (!respuesta.EsExitosa)
        {
            return BadRequest(respuesta);
        }

        var cursoCreado = respuesta.Datos;

        if (cursoCreado is null)
        {
            return BadRequest(respuesta);
        }

        var ubicacion = $"{RutaCursos}/{cursoCreado.Id}";

        return Created(ubicacion, respuesta);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Respuesta<CursoResponseDTO>>> Actualizar(int id, CursoUpdateDTO dto)
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
