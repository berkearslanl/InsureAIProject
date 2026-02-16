using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly InsureContext _context;

        public TestimonialController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult TestimonialList()
        {
            var values = _context.Testimonials.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddTestimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        [HttpGet]
        public IActionResult UpdateTestimonial(int id)
        {
            var values = _context.Testimonials.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }

        public async Task<IActionResult> CreateTestimonialWithClaudeAI()
        {
            string apikey = "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";

            string prompt = "Bir sigorta şirketi için Türkçe müşteri yorumları üret.Toplam 6 adet testimonial oluştur. Kurallar:Başlık yazma. Numara (1., 2., 3. gibi) kullanma. Listeleme yapma. Müşteri Yorumları gibi ifadeler kullanma. Her testimonial tek satır olsun.Format şu şekilde OLMALI:Yorum metni - Ad Soyad, Unvan. Sadece bu formatta 6 satır üret, başka hiçbir açıklama yazma.";

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.anthropic.com/");
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                max_tokens = 700,
                temperature = 0.5,
                messages = new[]
                {
                    new
                    {
                        role="user",
                        content=prompt
                    }
                }
            };

            var jsoncontent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.anthropic.com/v1/messages", jsoncontent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.testimonials = new List<string>
                {
                    $"Claude Api Hatası! Kod: {response.StatusCode} - Mesaj: {errorContent}"
                };
                return View();
            }

            var resposnestring = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resposnestring);
            var fulltext = doc.RootElement
                               .GetProperty("content")[0]
                               .GetProperty("text")
                               .GetString();

            var testimonials = fulltext.Split('\n')
                                   .Where(x => !string.IsNullOrEmpty(x))
                                   .Select(x => x.TrimStart('1', '2', '3', '4', '5', '.', ' '))
                                   .ToList();

            ViewBag.testimonials = testimonials;

            return View();

        }
    }
}
