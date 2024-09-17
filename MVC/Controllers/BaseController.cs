using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Configurar autentica��o no in�cio de cada requisi��o
            ViewData["IsAuthenticated"] = true; // Defina conforme a autentica��o do usu�rio
            base.OnActionExecuting(context);
        }
    }
}
