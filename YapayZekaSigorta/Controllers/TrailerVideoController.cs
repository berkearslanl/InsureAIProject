using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class TrailerVideoController : Controller
    {
        private readonly InsureContext _context;

        public TrailerVideoController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult TrailerVideoList()
        {
            var values = _context.TrailerVideos.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddTrailerVideo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTrailerVideo(TrailerVideo trailerVideo)
        {
            _context.TrailerVideos.Add(trailerVideo);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        }
        [HttpGet]
        public IActionResult UpdateTrailerVideo(int id)
        {
            var values = _context.TrailerVideos.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateTrailerVideo(TrailerVideo trailerVideo)
        {
            _context.TrailerVideos.Update(trailerVideo);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        }
        public IActionResult DeleteTrailerVideo(int id)
        {
            var value = _context.TrailerVideos.Find(id);
            _context.TrailerVideos.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("TrailerVideoList");
        }
    }
}
