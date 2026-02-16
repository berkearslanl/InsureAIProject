using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace YapayZekaSigorta.Controllers
{
    public class ImageAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageAIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult CreateImageWithOpenAI()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateImageWithOpenAI(string prompt)
        {
            var apikey = "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";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);

            var requestBody = new
            {
                model = "gpt-image-1",
                prompt = prompt,
                n = 1,
                size = "1024x1024"
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.error = "OpenAI Hatası: " + await response.Content.ReadAsStringAsync();
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(json);
            var base64Image = result.RootElement
                .GetProperty("data")[0]
                .GetProperty("b64_json")
                .GetString();

            var imageUrl = $"data:image/png;base64,{base64Image}";
            //var imageUrl = result.RootElement.GetProperty("data")[0].GetProperty("url").GetString();

            return View(model: imageUrl);
        }
    }
}
