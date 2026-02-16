using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace YapayZekaSigorta.Controllers
{
    public class TavilyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string tavilyApiKey = "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";
        private readonly string openAIApiKey = "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";


        public TavilyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> SearchWithTavily(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                ViewBag.error = "Lütfen bir arama sorgusu giriniz!";
                return View("Search");
            }

            var tavilyResponse = await CallTavilyAsync(query);

            var openAIResponse = await SummarizeWithOpenAI(query, tavilyResponse);

            ViewBag.Query = query;
            ViewBag.TavilyRaw = tavilyResponse;
            ViewBag.OpenAIResult = openAIResponse;

            return View("Search");
        }

        private async Task<string> CallTavilyAsync(string query)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.tavily.com/");

            var requestBody = new
            {
                api_key = tavilyApiKey,
                query = query,
                include_answer = true,
                max_results = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("search", content);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SummarizeWithOpenAI(string query, string tavilyJson)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            var prompt = $@"
Kullanıcının sorusu: {query}

Aşağıdaki Tavily web araması sonuçlarını oku ve kullanıcıya kısa, net ve akademik bir açıklama yap.
Önemli noktaları sade şekilde özetle. Gereksiz cümle kurma.

Tavily sonuçları:
{tavilyJson}
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result?.choices?[0]?.message?.content ?? "OpenAI yanıt üretemedi.";
        }
    }
}
