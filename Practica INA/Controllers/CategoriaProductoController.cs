using Microsoft.AspNetCore.Mvc;
using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Models;
using Practica_INA.Data.Models.DTOs;

namespace Practica_INA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaProductoController : ControllerBase
    {
        private readonly ICategoriaProductoService _service;

        public CategoriaProductoController(ICategoriaProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<CategoriaProductoResponseDTO>>>> ObtenerTodos()
        {
            var resultado = await _service.ObtenerTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<CategoriaProductoResponseDTO>>> ObtenerPorId(int id)
        {
            var resultado = await _service.ObtenerPorIdAsync(id);
            if (!resultado.Success)
            {
                return NotFound(resultado);
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CategoriaProductoResponseDTO>>> Crear([FromBody] CategoriaProductoCreateDTO dto)
        {
            var resultado = await _service.CrearAsync(dto);
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Data.Id }, resultado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<CategoriaProductoResponseDTO>>> Actualizar(int id, [FromBody] CategoriaProductoUpdateDTO dto)
        {
            dto.Id = id;
            var resultado = await _service.ActualizarAsync(dto);
            if (!resultado.Success)
            {
                return BadRequest(resultado);
            }
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Eliminar(int id)
        {
            var resultado = await _service.EliminarAsync(id);
            if (!resultado.Success)
            {
                return NotFound(resultado);
            }
            return Ok(resultado);
        }
    }
}
