using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardSubChartComponentPartial:ViewComponent
    {
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
