using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultServicesComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultServicesComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Services.ToList();
            return View(values);
        }
    }
}
