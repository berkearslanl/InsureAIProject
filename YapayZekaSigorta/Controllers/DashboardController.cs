using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
