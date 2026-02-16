using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListTagsComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
