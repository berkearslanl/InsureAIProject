using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DashboardViewComponent
{
    public class _DashboardCommentListComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DashboardCommentListComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Comments.Include(x => x.AppUser).OrderByDescending(x => x.CommentId).Take(7).ToList();
            return View(values);
        }
    }
}
