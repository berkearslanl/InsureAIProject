using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;
using YapayZekaSigorta.Models;

namespace YapayZekaSigorta.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly InsureContext _context;

        public PricingPlanController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult PricingPlanList()
        {
            var values = _context.PricingPlans.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddPricingPlan()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Add(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        [HttpGet]
        public IActionResult UpdatePricingPlan(int id)
        {
            var values = _context.PricingPlans.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdatePricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Update(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        public IActionResult DeletePricingPlan(int id)
        {
            var value = _context.PricingPlans.Find(id);
            _context.PricingPlans.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }
        public IActionResult ChangeFeatureStatus(int id)
        {
            var value = _context.PricingPlans.Find(id);
            if (value != null)
            {
                // Değer true ise false, false ise true yapar
                value.IsFeature = !value.IsFeature;
                _context.SaveChanges();
            }
            return RedirectToAction("PricingPlanList");
        }
        [HttpGet]
        public IActionResult CreateUserCustomizePlan()
        {
            var model = new AIInsuranceRecommendationViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserCustomizePlan(AIInsuranceRecommendationViewModel model)
        {
            string apikey = "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";
            // Kullanıcı girdilerini JSON'a çeviriyoruz
            var userJson = JsonConvert.SerializeObject(model);

            // OpenAI'ye göndereceğimiz prompt:
            var prompt = $@"
Sen profesyonel bir sigorta uzmanı AI asistanısın. 
Aşağıdaki kullanıcının bilgilerini analiz ederek en uygun sigorta paketini öner.

Paketler ve özellikleri:
1) Premium Paket (599 TL/ay): Yatarak tedavi, check-up, geniş yol yardım, yurtiçi seyahat güvencesi.
2) Standart Paket (449 TL/ay): Acil sağlık, müşteri hizmetleri, kaza sonrası tıbbi destek.
3) Ekonomik Paket (339 TL/ay): Temel sağlık, temel yol yardım.

Kullanıcı bilgileri:
{userJson}

Sadece şu formatta JSON döndür:

{{
  ""onerilenPaket"": ""Premium | Standart | Ekonomik"",
  ""ikinciSecenek"": ""Premium | Standart | Ekonomik"",
  ""neden"": ""Kısa analiz metni""
}}
";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apikey);

            var body = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            dynamic ai = JsonConvert.DeserializeObject(jsonResponse);
            string aiResult = ai.choices[0].message.content;

            // AI cevabı JSON formatında gelmiş olacak
            var result = JsonConvert.DeserializeObject<AIInsuranceRecommendationViewModel>(aiResult);

            // Sonuçları modele geri yazıyoruz
            model.RecommendedPackage = result.onerilenPaket;
            model.SecondBestPackage = result.ikinciSecenek;
            model.AnalysisText = result.neden;

            return View(model);
        }
    }
}
