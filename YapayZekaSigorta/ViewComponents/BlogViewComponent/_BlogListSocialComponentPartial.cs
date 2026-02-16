using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListSocialComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
