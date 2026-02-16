using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.BlogViewComponent
{
    public class _BlogListByCategoryComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _BlogListByCategoryComponentPartial(InsureContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            var values = _context.Articles.Include(x => x.Category).Include(x=>x.AppUser).Where(y => y.CategoryId == id).ToList();
            return View(values);
        }
    }
}
