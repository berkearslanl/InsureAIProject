using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultScriptsComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
