using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class AboutItemController : Controller
    {
        private readonly InsureContext _context;

        public AboutItemController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult AboutItemList()
        {
            var values = _context.AboutItems.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddAboutItem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Add(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        [HttpGet]
        public IActionResult UpdateAboutItem(int id)
        {
            var values = _context.AboutItems.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Update(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        public IActionResult DeleteAboutItem(int id)
        {
            var value = _context.AboutItems.Find(id);
            _context.AboutItems.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAboutItemWithGemini()
        {
            var apikey = "AIzaSyDbr8mUmS8Pndqj2GDmpYtZ7ZmXlCqdeGo";
            var model = "gemini-2.5-flash";
            ViewBag.model = model;
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
                                text="Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkımızda alanları(about item)' yazısı oluştur. Örneğin 'Geleceğinizi güvence altına alan kapsamlı sigorta çözümleri sunuyoruz.' şeklinde veya bunun gibi ve buna benzer daha zengin içerikler gelsin. Maddelerin açıklamalarını oluşturmana gerek yok sadece madde olsa yeterli. En az 10 tane istiyorum. Metni Markdown (yıldız işaretleri) kullanmadan, doğrudan HTML etiketleri (<b>, <h4> vb.) kullanarak döndür. Metnin başlangıcında 'İşte hakkımızda alanları yazısı' gibi ifadeler kullanmadan dişrekt hakkımızda alanları yazılarını ver."
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
