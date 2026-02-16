using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.AdminLayoutViewComponent
{
    public class _AdminLayoutSidebarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
