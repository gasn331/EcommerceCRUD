using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Configurar autenticação no início de cada requisição
            ViewData["IsAuthenticated"] = true; // Defina conforme a autenticação do usuário
            base.OnActionExecuting(context);
        }
    }
}
