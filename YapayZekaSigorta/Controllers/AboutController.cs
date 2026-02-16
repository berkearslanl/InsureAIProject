using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class AboutController : Controller
    {
        private readonly InsureContext _context;

        public AboutController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult AboutList()
        {
            var values = _context.Abouts.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddAbout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAbout(About about)
        {
            _context.Abouts.Add(about);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }
        [HttpGet]
        public IActionResult UpdateAbout(int id)
        {
            var values = _context.Abouts.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateAbout(About about)
        {
            _context.Abouts.Update(about);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAboutWithGemini()
        {
            var apikey = "AIzaSyDbr8mUmS8Pndqj2GDmpYtZ7ZmXlCqdeGo";
            var model = "gemini-2.5-flash";
            var url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apikey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts=new[]
                        {
                            new
                            {
                                text="Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkımızda' yazısı oluştur. Metni Markdown (yıldız işaretleri) kullanmadan, doğrudan HTML etiketleri (<b>, <h4> vb.) kullanarak döndür. Metnin başlangıcında 'İşte hakkımızda yazısı' gibi ifadeler kullanmadan dişrekt hakkımızda yazısını ver."
                            }
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var responseJson = await response.Content.ReadAsStringAsync();

            using var jsondoc = JsonDocument.Parse(responseJson);
            var abouttext = jsondoc.RootElement
                            .GetProperty("candidates")[0]
                            .GetProperty("content")
                            .GetProperty("parts")[0]
                            .GetProperty("text")
                            .GetString();
            ViewBag.abouttext = abouttext;
            return View();
        }
    }
}
//AIzaSyDaQ9nrwPI3V6-sDerW-hdQlLGrqRYBCSg

