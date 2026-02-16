using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultAboutComponentPartial : ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultAboutComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.title = _context.Abouts.Select(x => x.Title).FirstOrDefault();
            ViewBag.description = _context.Abouts.Select(x => x.Description).FirstOrDefault();
            ViewBag.imageurl = _context.Abouts.Select(x => x.ImageUrl).FirstOrDefault();

            var aboutitemvalues = _context.AboutItems.Take(3).ToList();
            return View(aboutitemvalues);
        }
    }
}
