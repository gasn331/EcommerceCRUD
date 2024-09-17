using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Definir o estado de autentica��o como n�o autenticado
            ViewData["IsAuthenticated"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ValidateUserAsync(model);

                if (result)
                {
                    // Definir o estado de autentica��o como autenticado e redirecionar
                    ViewData["IsAuthenticated"] = true;
                    return RedirectToAction("Index", "Produto");
                }

                ModelState.AddModelError("", "Email ou senha inv�lidos.");
            }
            else
            {
                ModelState.AddModelError("", "Os dados fornecidos s�o inv�lidos.");
            }

            ViewData["IsAuthenticated"] = false;
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Definir o estado de autentica��o como n�o autenticado e redirecionar
            ViewData["IsAuthenticated"] = false;
            return RedirectToAction("Login", "User");
        }

        [HttpGet("User/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("User/Register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);

                if (result)
                {
                    return RedirectToAction("Login", "User");
                }

                ModelState.AddModelError("", "Falha ao criar o usu�rio.");
            }
            else
            {
                ModelState.AddModelError("", "Os dados fornecidos s�o inv�lidos.");
            }

            return View(model);
        }
    }
}
