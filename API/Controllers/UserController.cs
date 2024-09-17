using API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.CreateUserAsync(request.Email, request.Password);

            if (result)
            {
                return Ok("Usuário criado com sucesso!");
            }

            return BadRequest("Falha na criação do usuário");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuarioValido = await _userService.ValidateUserAsync(request.Email, request.Password);

            if(usuarioValido)
            {
                return Ok("Usuario válido");
            }

            return BadRequest("Usuário inválido");
        }
    }
}
