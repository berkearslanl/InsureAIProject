using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.BlogDetailViewComponent
{
    public class _BlogDetailNextAndPrevComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogDetailNextAndPrevComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int id)
        {
            //mevcut makale
            var article = _context.Articles.FirstOrDefault(x => x.ArticleId == id);

            //önceki makale
            var prevArticle = _context.Articles
                .Where(x => x.ArticleId < id)
                .OrderByDescending(x => x.ArticleId)
                .Select(x => new { x.Title,x.ArticleId})
                .FirstOrDefault();

            //sonraki makale
            var nextArticle = _context.Articles
                .Where(x => x.ArticleId > id)
                .OrderBy(x => x.ArticleId)
                .Select(x => new { x.Title, x.ArticleId })
                .FirstOrDefault();

            ViewBag.prevArticle = prevArticle?.Title;
            ViewBag.prevArticleid = prevArticle?.ArticleId;

            ViewBag.nextArticle = nextArticle?.Title;
            ViewBag.nextArticleid = nextArticle?.ArticleId;
            return View();
        }
    }
}
