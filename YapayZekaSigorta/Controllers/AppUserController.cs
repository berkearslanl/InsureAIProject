using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.Controllers
{
    public class AppUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly InsureContext _context;

        public AppUserController(UserManager<AppUser> userManager, InsureContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult UserList()
        {
            var values = _userManager.Users.ToList();
            return View(values);
        }
        public async Task<IActionResult> UserProfileWithAI(string id)
        {
            //KULLANICI BİLGİLERİNİ PROFİL EKRANINA GETİRME
            var values = await _userManager.FindByIdAsync(id);
            ViewBag.name = values.Name;
            ViewBag.surname = values.Surname;
            ViewBag.description = values.Description;
            ViewBag.image = values.ImageUrl;
            ViewBag.titlevalue = values.Title;
            ViewBag.education = values.Education;
            ViewBag.city = values.City;


            

            //MAKALE ANALİZİ
            //kullanıcı bilgilerini çekme
            var user = await _userManager.FindByIdAsync(id);
            //eğer kullanıcı id'sini çekemediysek(kullanıcı yoksa)
            if (user == null)
            {
                return NotFound();
            }
            //kullanıcıya ait makale listesi
            var articles = await _context.Articles
                .Where(x => x.AppUser.Id == id)
                .Select(y => y.Content)
                .ToListAsync();
            //eğer çekilen kullanıcıya ait bir makale yoks
            if (articles.Count == 0)
            {
                ViewBag.AIRESULT = "Bu kullanıcıya ait analiz yapılacak bir makale bulunamadı!";
                return View(user);
            }
            //makaleleri tek bir metinde toplayalım
            var allArticles = string.Join("\n\n", articles);

            var apiKey = "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";

            //promptun yazılması
            var prompt = $@"
Siz bir sigorta sektöründe uzman bir içerik analistiğisin.
Elinizde, bir sigorta şirketinin çalışanının yazdığı tüm makaleler var.
Bu makaleler üzerinden çalışanın içerik üretim tarzını analiz et.

Analiz Başlıkları:

1) Konu çeşitliliği ve odak alanları(sağlık, hayat, kasko, tamamlayıcı, BES vb.)
2) Hedef kitle tahmini (bireysel / kurumsal, segment, persona)
3) Dil ve anlatım tarzı (tekniklik seviyesi, okunabilirlik ve ikna gücü)
4) Sigorta terimlerini kullanma ve doğruluk düzeyi
5) Müşteri ihtiyaçlarına ve risk yönetimine odaklanma
6) Pazarlama / Satış vurgusu, CTA netliği
7) Geliştirilmesi gereken alanlar ve net aksiyon maddeleri

Makaleler:
{allArticles}

Lütfen çıktıyı profesyonel rapor formatında, madde madde ve en sonda 5 maddelik aksiyon listesi ile ver.";

            //OPENAI Chat Completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new{role="system",content="Sen sigorta sektöründe içerik analizi yapan bir uzmansın."},
                    new{role="user",content=prompt}
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //json dönüşümleri
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseText = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                ViewBag.AIRESULT = "Open AI Hatası: " + httpResponse.StatusCode;
            }
            //json yapı içinden veriyi okuma işlemi
            try
            {
                using var doc = JsonDocument.Parse(responseText);
                var aiText = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();
                ViewBag.AIRESULT = aiText ?? "Boş yanıt döndü!";
            }
            catch
            {

                ViewBag.AIRESULT = "Open AI Yanıtı beklenen formatta değil.";
            }

            return View();
        }

        public async Task<IActionResult> UserCommentsProfileWithAI(string id)
        {
            //KULLANICI BİLGİLERİNİ PROFİL EKRANINA GETİRME
            var values = await _userManager.FindByIdAsync(id);
            ViewBag.name = values.Name;
            ViewBag.surname = values.Surname;
            ViewBag.description = values.Description;
            ViewBag.image = values.ImageUrl;
            ViewBag.titlevalue = values.Title;
            ViewBag.education = values.Education;
            ViewBag.city = values.City;

            //MAKALE ANALİZİ
            //kullanıcı bilgilerini çekme
            var user = await _userManager.FindByIdAsync(id);
            //eğer kullanıcı id'sini çekemediysek(kullanıcı yoksa)
            if (user == null)
            {
                return NotFound();
            }
            //kullanıcıya ait makale listesi
            var comments = await _context.Comments
                .Where(x => x.AppUser.Id == id)
                .Select(y => y.CommentDetail)
                .ToListAsync();
            //eğer çekilen kullanıcıya ait bir makale yoks
            if (comments.Count == 0)
            {
                ViewBag.AIRESULT = "Bu kullanıcıya ait analiz yapılacak bir yorum bulunamadı!";
                return View(user);
            }
            //makaleleri tek bir metinde toplayalım
            var allComments = string.Join("\n\n", comments);

            var apiKey = "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";

            //promptun yazılması
            var prompt = $@"
Sen kullanıcı davranış analizi yapan bir yapay zeka uzmanısın.
Aşağıdaki yorumlara göre kullanıcıyı değerlendir.

Analiz Başlıkları:

1) Genel Duygu Durumu (pozitif/negatif/nötr)
2) Toksik içerik var mı? (Örnekleriyle)
3) İlgi alanları / konu başlıkları
4) İletişim tarzı (samimi, resmi, agresif vb.)
5) Geliştirilmesi gereken iletişim alanları
6) 5 maddelik kısa özet

Yorumlar:
{allComments}";

            //OPENAI Chat Completions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var body = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new{role="system",content="Sen kullanıcı yorum analizi yapan bir uzmansın."},
                    new{role="user",content=prompt}
                },
                max_tokens = 1000,
                temperature = 0.2
            };

            //json dönüşümleri
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseText = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                ViewBag.AIRESULT = "Open AI Hatası: " + httpResponse.StatusCode;
            }
            //json yapı içinden veriyi okuma işlemi
            try
            {
                using var doc = JsonDocument.Parse(responseText);
                var aiText = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();
                ViewBag.AIRESULT = aiText ?? "Boş yanıt döndü!";
            }
            catch
            {

                ViewBag.AIRESULT = "Open AI Yanıtı beklenen formatta değil.";
            }

            return View();
        }
    }
}
