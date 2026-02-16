using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListAllBlogsComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListAllBlogsComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            //var values = _context.Articles.Include(x => x.Category).Include(y=>y.AppUser).ToList();
            var articles = _context.Articles
                .Include(x => x.Category)
                .Include(y => y.AppUser)
                .Include(z => z.Comments)
                .Select(p => new ArticleListViewModel
                {
                    ArticleId = p.ArticleId,
                    Author = p.AppUser.Name + " " + p.AppUser.Surname,
                    CategoryName = p.Category.CategoryName,
                    CreatedDate = p.CreatedDate,
                    Description = p.Content,
                    ImageUrl = p.CoverImageUrl,
                    CommentCount = p.Comments.Count,
                    Title = p.Title,
                }).ToList();
            return View(articles);
        }
    }
}
