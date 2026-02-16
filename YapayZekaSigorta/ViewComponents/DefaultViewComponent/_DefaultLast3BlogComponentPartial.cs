using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultLast3BlogComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultLast3BlogComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Articles.OrderByDescending(x => x.ArticleId).Include(y => y.Category).Include(y=>y.AppUser).Take(3).ToList();
            return View(values);
        }
    }
}
