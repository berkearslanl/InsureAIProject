using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardSubWidgetComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubWidgetComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.messageCount = _context.Messages.Count();
            ViewBag.avgPolicyAmount = _context.Policies.Average(x => x.PremiumAmount).ToString("0.00");
            ViewBag.lastRevenueAmount = _context.Revenues.OrderByDescending(x => x.RevenueId).Take(1).Select(y => y.Amount).FirstOrDefault().ToString("0.00");

            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startOfNextMonth = startOfMonth.AddMonths(1);

            ViewBag.totalPolicyThisMonth = _context.Policies.Where(x => x.CreatedDate >=startOfMonth && x.CreatedDate<startOfNextMonth).Count();

            ViewBag.testimonialCount = _context.Testimonials.Count();
            ViewBag.subscribeCount = _context.NewsLetters.Count();
            ViewBag.serviceCount = _context.Services.Count();


            return View();
        }
    }
}
