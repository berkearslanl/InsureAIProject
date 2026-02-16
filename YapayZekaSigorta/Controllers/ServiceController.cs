using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class ServiceController : Controller
    {
        private readonly InsureContext _context;

        public ServiceController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult ServiceList()
        {
            var values = _context.Services.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }
        [HttpGet]
        public IActionResult UpdateService(int id)
        {
            var values = _context.Services.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateService(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }
        public IActionResult DeleteService(int id)
        {
            var value = _context.Services.Find(id);
            _context.Services.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        public async Task<IActionResult> CreateServiceWithAnthropicClaude()
        {
            string apikey = "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";

            string prompt = "Bir sigorta şirketi için hizmetler bölümü hazırlamanı istiyorum. Burada 5 farklı hizmet olmalı. Bana maksimum 100 karakterden oluışan cümlelerle 5 tane hizmet içeriği yazar mısın?";

            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.anthropic.com/");
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                max_tokens = 512,
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
                ViewBag.services = new List<string>
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

            var services = fulltext.Split('\n')
                                   .Where(x => !string.IsNullOrEmpty(x))
                                   .Select(x => x.TrimStart('1', '2', '3', '4', '5', '.', ' '))
                                   .ToList();

            ViewBag.services = services;

            return View();
        }
    }
}
