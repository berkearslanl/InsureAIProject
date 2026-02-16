using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
