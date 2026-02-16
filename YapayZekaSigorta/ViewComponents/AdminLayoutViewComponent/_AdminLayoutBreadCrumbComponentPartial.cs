using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.AdminLayoutViewComponent
{
    public class _AdminLayoutBreadCrumbComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
