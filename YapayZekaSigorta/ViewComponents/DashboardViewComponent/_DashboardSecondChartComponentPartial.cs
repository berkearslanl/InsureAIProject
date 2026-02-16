using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardSecondChartComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSecondChartComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var activeCount = _context.Policies.Count(x => x.Status == "Aktif");
            var expiredCount = _context.Policies.Count(x => x.Status == "Süresi Doldu");

            var model = new PolicyStatusChartViewModel
            {
                ActiveCount = activeCount,
                ExpiredCount = expiredCount
            };
            return View(model);
        }
    }
}
