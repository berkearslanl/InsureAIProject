using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultNavbarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
