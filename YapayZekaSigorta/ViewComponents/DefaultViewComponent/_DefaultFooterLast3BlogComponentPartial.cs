using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultFooterLast3BlogComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultFooterLast3BlogComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Articles.OrderByDescending(x => x.ArticleId).Skip(3).Take(2).ToList();
            return View(values);
        }
    }
}
