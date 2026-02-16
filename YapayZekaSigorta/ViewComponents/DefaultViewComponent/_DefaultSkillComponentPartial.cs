using Microsoft.AspNetCore.Mvc;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultSkillComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
