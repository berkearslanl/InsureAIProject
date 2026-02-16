using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardSubChart1ComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardSubChart1ComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var policyData = _context.Policies
                .GroupBy(p => p.PolicyType)
                .Select(g => new 
                {
                    PolicyType = g.Key,
                    Count = g.Count()
                })
                .ToList();

            ViewBag.policyData = JsonConvert.SerializeObject(policyData);
            ViewBag.policyCount = _context.Policies.Count();
            return View();
        }
    }
}
