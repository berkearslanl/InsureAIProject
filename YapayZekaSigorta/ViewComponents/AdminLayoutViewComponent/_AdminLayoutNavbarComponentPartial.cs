using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.AdminLayoutViewComponent
{
    public class _AdminLayoutNavbarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
