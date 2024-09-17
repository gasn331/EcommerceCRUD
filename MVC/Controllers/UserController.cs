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
            // Definir o estado de autenticação como não autenticado
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
                    // Definir o estado de autenticação como autenticado e redirecionar
                    ViewData["IsAuthenticated"] = true;
                    return RedirectToAction("Index", "Produto");
                }

                ModelState.AddModelError("", "Email ou senha inválidos.");
            }
            else
            {
                ModelState.AddModelError("", "Os dados fornecidos são inválidos.");
            }

            ViewData["IsAuthenticated"] = false;
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Definir o estado de autenticação como não autenticado e redirecionar
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

                ModelState.AddModelError("", "Falha ao criar o usuário.");
            }
            else
            {
                ModelState.AddModelError("", "Os dados fornecidos são inválidos.");
            }

            return View(model);
        }
    }
}
