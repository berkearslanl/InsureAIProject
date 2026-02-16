using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class MessageController : Controller
    {
        private readonly InsureContext _context;

        public MessageController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult MessageList()
        {
            var values = _context.Messages.ToList();
            return View(values);
        }
        public IActionResult DeleteMessage(int id)
        {
            var value = _context.Messages.Find(id);
            _context.Messages.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("MessageList");
        }
        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
                return NotFound();

            message.IsRead = true;
            _context.SaveChanges();

            return Json(new { success = true });
        }

    }
}
