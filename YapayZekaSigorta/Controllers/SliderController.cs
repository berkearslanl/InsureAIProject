using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class SliderController : Controller
    {
        private readonly InsureContext _context;

        public SliderController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult SliderList()
        {
            var values = _context.Sliders.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddSlider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddSlider(Slider slider)
        {
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
        [HttpGet]
        public IActionResult UpdateSlider(int id)
        {
            var values = _context.Sliders.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateSlider(Slider slider)
        {
            _context.Sliders.Update(slider);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
        public IActionResult DeleteSlider(int id)
        {
            var value = _context.Sliders.Find(id);
            _context.Sliders.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("SliderList");
        }
    }
}
