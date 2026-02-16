using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultPricingPlanComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultPricingPlanComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.title = _context.PricingPlans.Select(x => x.Title).FirstOrDefault();
            ViewBag.description1 = _context.PricingPlans.Select(x => x.Description1).FirstOrDefault();
            ViewBag.description2 = _context.PricingPlans.Select(x => x.Description2).FirstOrDefault();

            //1. plan
            var pricingPlan1 = _context.PricingPlans.Where(x=>x.IsFeature==true).FirstOrDefault();
            ViewBag.plan1name = pricingPlan1.PlanName;
            ViewBag.plan1price = pricingPlan1.Price;
            ViewBag.plan1Id = pricingPlan1.PricingPlanId;
            //2. plan
            var pricingPlan2 = _context.PricingPlans.Where(x => x.IsFeature == true).OrderByDescending(y => y.PricingPlanId).FirstOrDefault();
            ViewBag.plan2name = pricingPlan2.PlanName;
            ViewBag.plan2price = pricingPlan2.Price;
            ViewBag.plan2Id = pricingPlan2.PricingPlanId;

            var pricingPlanItems = _context.PricingPlanItems.Where(x => x.PricingPlanId == pricingPlan1.PricingPlanId || x.PricingPlanId == pricingPlan2.PricingPlanId).ToList();

            return View(pricingPlanItems);
        }
    }
}
