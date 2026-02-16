using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardWidgetComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardWidgetComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            int r1, r2, r3, r4;
            int n1, n2, n3, n4;
            Random rnd = new Random();
            r1 = rnd.Next(0, 10);
            n1 = rnd.Next(1, 30);

            r2 = rnd.Next(0, 10);
            n2 = rnd.Next(1, 30);

            r3 = rnd.Next(0, 10);
            n3 = rnd.Next(1, 30);

            r4 = rnd.Next(0, 10);
            n4 = rnd.Next(1, 30);

            ViewBag.v1 = _context.Articles.Count();
            ViewBag.v2 = _context.Categories.Count();
            ViewBag.v3 = _context.Comments.Count();
            ViewBag.v4 = _context.Users.Count();

            ViewBag.r1 = n1 + "." + r1;
            ViewBag.r2 = n2 + "." + r2;
            ViewBag.r3 = n3 + "." + r3;
            ViewBag.r4 = n4 + "." + r4;
            return View();
        }
    }
}
