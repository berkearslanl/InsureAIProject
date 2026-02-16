using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardAppUserQuickViewTableComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardAppUserQuickViewTableComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Users
                .GroupJoin
                (
                _context.Policies,
                user => user.Id,
                policy => policy.AppUserId,
                (user, policies) => new UserPolicySummaryViewModel
                {
                    UserId = user.Id,
                    FullName = user.Name + " " + user.Surname,
                    ImageUrl = user.ImageUrl,
                    PolicyCount = policies.Count(),
                    TotalPremium = policies.Sum(p => (decimal?)p.PremiumAmount) ?? 0
                })
                .OrderByDescending(x => x.PolicyCount)
                .ToList();
            return View(values);
        }
    }
}
