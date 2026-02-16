
using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.AdminLayoutViewComponent
{
    public class _AdminLayoutHeadComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
