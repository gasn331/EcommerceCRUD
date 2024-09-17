using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private IDepartamentoService _departamentoService;

        public DepartamentoController(IDepartamentoService departamentoService) 
        {
            _departamentoService = departamentoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> GetDepartamentos() 
        {
            var departamentos = await _departamentoService.GetAllDepartamentosAsync();

            return Ok(departamentos);
        }

        //GET: api/departamento/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<DepartamentoDTO>> GetDepartamento(string codigo) 
        {
            var departamento = await _departamentoService.GetDepartamentoAsync(codigo);

            if(departamento == null)
            {
                return NotFound();
            }

            return Ok(departamento);
        }

    }
}
