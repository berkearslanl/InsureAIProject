using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultTrailerVideoComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultTrailerVideoComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.TrailerVideos.ToList();
            return View(values);
        }
    }
}
