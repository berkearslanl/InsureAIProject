using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UglyToad.PdfPig;

namespace YapayZekaSigorta.Controllers
{
    public class PolicyAnalysisController : Controller
    {
        private readonly string apikey = "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";
        [HttpGet]
        public IActionResult PdfAnalyze()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PdfAnalyze(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                ViewBag.error = "Lütfen bir PDF poliçe dosyası yükleyiniz.";
                return View();
            }

            string extractedText = await ExtractTextFromPdf(pdfFile);
            if (string.IsNullOrWhiteSpace(extractedText))
            {
                ViewBag.error = "PDF içerisinden metin çıkarılamadı.";
                return View();
            }

            string analysis = await AnalyzePolicyWithClaude(extractedText);

            ViewBag.originalText = extractedText;
            ViewBag.analysisResult = analysis;

            return View();
        }
        private async Task<string> ExtractTextFromPdf(IFormFile pdfFile)
        {
            using var ms = new MemoryStream();
            await pdfFile.CopyToAsync(ms);
            ms.Position = 0;

            var sb = new StringBuilder();

            using (var document = PdfDocument.Open(ms))
            {
                foreach (var page in document.GetPages())
                {
                    sb.AppendLine(page.Text);
                    sb.AppendLine("\n");
                }
            }

            return sb.ToString();
        }

        private async Task<string> AnalyzePolicyWithClaude(string policyText)
        {
            var apiUrl = "https://api.anthropic.com/v1/messages";

            var prompt = $@"
Aşağıdaki metin bir sigorta poliçesine aittir.

Görevlerin:
1) Poliçeyi 10 maddede özetle.
2) Neleri kapsar? (Madde madde yaz)
3) Neleri kapsamaz? (Madde madde yaz)
4) Müşteri için kritik uyarıları **kalın** yap.
5) Yanıtı markdown formatında üret.

--- POLİÇE METNİ ---
{policyText}
--- SON ---
";

            var body = new
            {
                model = "claude-sonnet-4-20250514",
                max_tokens = 4096,
                temperature = 0.2,
                messages = new[]
                {
                    new {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(body);

            using var http = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("x-api-key", apikey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"AI Analizi sırasında hata oluştu: {response.StatusCode}\n{responseText}";
            }

            using var doc = JsonDocument.Parse(responseText);
            var root = doc.RootElement;

            var contentArray = root.GetProperty("content");
            var resultSb = new StringBuilder();

            foreach (var item in contentArray.EnumerateArray())
            {
                if (item.GetProperty("type").GetString() == "text")
                {
                    resultSb.AppendLine(item.GetProperty("text").GetString());
                }
            }

            return resultSb.ToString();
        }
    }

}

