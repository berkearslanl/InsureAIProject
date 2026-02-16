using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.Controllers
{
    public class AdminLayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
