using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.BlogDetailViewComponent
{
    public class _BlogDetailContentComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailContentComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            //var values = _context.Articles.Where(x => x.ArticleId == id).ToList();
            var values = _context.Articles.Include(y => y.AppUser).Include(y => y.Category).Where(x => x.ArticleId == id).FirstOrDefault();
            ViewBag.commentcount = _context.Comments.Where(x => x.ArticleId == id).Count();
            return View(values);
        }
    }
}
