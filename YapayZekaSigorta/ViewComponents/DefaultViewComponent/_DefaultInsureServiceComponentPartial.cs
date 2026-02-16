using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultInsureServiceComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
