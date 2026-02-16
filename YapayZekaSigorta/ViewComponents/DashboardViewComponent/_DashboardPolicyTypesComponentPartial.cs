using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardPolicyTypesComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardPolicyTypesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var result = _context.Policies
                .GroupBy(x => x.PolicyType)
                .Select(a => new PolicyGroupViewModel
                {
                    PolicyType = a.Key,
                    Count = a.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            ViewBag.totalPolicyCount = result.Sum(x => x.Count);

            return View(result);
        }
    }
}
