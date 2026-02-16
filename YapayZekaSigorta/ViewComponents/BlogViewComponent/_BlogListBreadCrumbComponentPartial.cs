using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListBreadCrumbComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
