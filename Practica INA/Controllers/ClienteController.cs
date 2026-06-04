using Microsoft.AspNetCore.Mvc;
using Practica_INA.Business.Interfaces;
using Practica_INA.Data.Models;

namespace Practica_INA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clientes = _service.GetAll();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var cliente = _service.GetById(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public IActionResult Add(Cliente cliente)
        {
            _service.Add(cliente);
            return Ok(cliente);
        }

        [HttpPut]
        public IActionResult Update(Cliente cliente)
        {
            _service.Update(cliente);
            return Ok(cliente);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
    }
}
