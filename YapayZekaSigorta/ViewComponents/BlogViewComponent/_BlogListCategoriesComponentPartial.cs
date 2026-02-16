using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListCategoriesComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListCategoriesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var values = _context.Categories.ToList();
            var categories = _context.Categories.Select(c => new CategoryArticleCountViewModel
            {
                ArticleCount = c.Articles.Count(),
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToList();
            return View(categories);
        }
    }
}
