using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;

namespace YapayZekaSigorta.ViewComponents.DefaultViewComponent
{
    public class _DefaultFooterComponentPartial:ViewComponent
    {
        private readonly InsureContext _context;

        public _DefaultFooterComponentPartial(InsureContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.description = _context.Contacts.Select(x => x.Description).FirstOrDefault();
            ViewBag.phone = _context.Contacts.Select(x => x.Phone).FirstOrDefault();
            ViewBag.email = _context.Contacts.Select(x => x.Email).FirstOrDefault();
            ViewBag.adress = _context.Contacts.Select(x => x.Adress).FirstOrDefault();
            return View();
        }
    }
}
