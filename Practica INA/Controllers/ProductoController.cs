using Microsoft.AspNetCore.Mvc;
using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var productos = _service.GetAll();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var producto = _service.GetById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public IActionResult Add(Producto producto)
        {
            _service.Add(producto);
            return Ok(producto);
        }

        [HttpPut]
        public IActionResult Update(Producto producto)
        {
            _service.Update(producto);
            return Ok(producto);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
