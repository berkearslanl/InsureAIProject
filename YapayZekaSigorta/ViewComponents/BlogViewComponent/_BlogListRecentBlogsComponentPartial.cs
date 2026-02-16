using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListRecentBlogsComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListRecentBlogsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Articles.OrderByDescending(x => x.ArticleId).Take(3).ToList();
            return View(values);
        }
    }
}
