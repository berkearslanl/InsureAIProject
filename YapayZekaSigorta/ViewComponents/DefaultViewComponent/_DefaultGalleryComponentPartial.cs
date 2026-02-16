using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultGalleryComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultGalleryComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Galleries.ToList();
            return View(values);
        }
    }
}
