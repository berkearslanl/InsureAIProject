using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultTestimonialComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultTestimonialComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Testimonials.ToList();
            return View(values);
        }
    }
}
