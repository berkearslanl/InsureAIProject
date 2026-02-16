using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Errors/404")]
        public IActionResult Page404()
        {
            return View();
        }
    }
}
