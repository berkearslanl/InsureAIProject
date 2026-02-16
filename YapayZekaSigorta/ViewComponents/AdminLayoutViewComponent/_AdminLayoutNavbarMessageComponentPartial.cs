using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.AdminLayoutViewComponent
{
    public class _AdminLayoutNavbarMessageComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
