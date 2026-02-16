using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;
using YapayZekaSigorta.Services;

namespace YapayZekaSigorta.Controllers
{
    public class DefaultController : Controller
    {
        private readonly InsureContext _context;
        private readonly AIService _aiService;

        public DefaultController(InsureContext context, AIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            #region Claude_AI_Analiz

            string apiKey = "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";
            string prompt = $"Sen bir sigorta şirketinin resmi dijital müşteri temsilcisisin.\r\n\r\nSigorta şirketinin adı 'InsureAI'.\r\nŞirkete gelen tüm mesajlara (e-posta, web formu, sosyal medya, WhatsApp vb.) profesyonel, net ve güven veren şekilde yanıt verirsin.\r\n\r\nİletişim dili:\r\n\r\nKurumsal ama samimi.\r\n\r\nNet ve sade.\r\n\r\nGereksiz uzunluk ve teknik karmaşa yok.\r\n\r\nHasar veya mağduriyet durumlarında empatik ol.\r\n\r\nŞikayetlerde savunmaya geçme, çözüm odaklı ol.\r\n\r\nEksik bilgi varsa varsayım yapma, nazikçe talep et.\r\n\r\nKapsam:\r\n\r\nPoliçe teklifi\r\n\r\nMevcut poliçe bilgisi\r\n\r\nHasar bildirimi ve takibi\r\n\r\nTeminat kapsamı\r\n\r\nÖdeme ve yenileme işlemleri\r\n\r\nİptal talepleri\r\n\r\nŞikayet ve genel bilgi talepleri\r\n\r\nYanıt yapısı:\r\n\r\nNazik hitap\r\n\r\nKonunun anlaşıldığını gösteren kısa ifade\r\n\r\nAçık ve net çözüm/süreç açıklaması\r\n\r\nGerekirse istenen ek bilgi\r\n\r\nDestek vurgusu ile kapanış\r\n\r\nHukuki veya finansal konularda kesinlik gerektiren durumlarda net olmayan bilgi üretme. Bilinmeyen konuda tahmin yapma.\r\n\r\nHer zaman şirket itibarını ve müşteri güvenini koruyarak yanıt ver.\r\n\r\nCevaplarını teşekkür ve iyi dilekle bitir.\r\n\r\nKullanıcının sana gönderdiği mesaj şu şekilde: ' {message.MessageDetail}. '";

            using var httpclient = new HttpClient();
            httpclient.BaseAddress = new Uri("https://api.anthropic.com/");
            httpclient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            httpclient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-3-haiku-20240307",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.5
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await httpclient.PostAsync("v1/messages", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            var json = JsonNode.Parse(responseString);
            string? textContent = json?["content"]?[0]?["text"]?.ToString();

            //ViewBag.v = textContent ?? "Yanıt alınamadı.";


            #endregion

            #region MailGönderme

            //yapay zeka ile gelen mesaja göre mail ile yanıt gönderme
            MimeMessage mimeMessage = new MimeMessage();
            //gönderici bilgileri
            MailboxAddress mailboxAddressFrom = new MailboxAddress("Insure AI Admin", "berkesude39@gmail.com");
            mimeMessage.From.Add(mailboxAddressFrom);
            //alıcı bilgileri
            MailboxAddress mailboxAddressTo = new MailboxAddress(message.NameSurname, message.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            //içerik bilgileri
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = textContent;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "InsureAI Yanıtı";

            //maili gönderme
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);//587 türkiye için, false ise ssl kullanılsın mı için
            client.Authenticate("berkesude39@gmail.com", "gflk lqea xcfa mtlp");
            client.Send(mimeMessage);
            client.Disconnect(true);
            #endregion

            #region MesajlarıKategorizeEtme
            var combinedText = $"{message.Subject} - {message.MessageDetail}";
            var predictedCategory = await _aiService.PredictCategoryAsync(combinedText);
            #endregion

            #region MesajlardaÖncelikBelirleme
            var priority = await _aiService.PredictPriorityAsync(combinedText);
            #endregion

            #region VeritabanınaKayıtİşlemleri
            message.Priority = priority;
            message.AICategory = predictedCategory;
            message.SendDate = DateTime.Now;
            message.AIAnswer = textContent;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["ToastType"] = "success";
            TempData["Başarılı"] = "Mesajınız başarıyla iletildi!";
            return RedirectToAction("Index");
            #endregion
        }
        [HttpGet]
        public PartialViewResult SubscribeEmail()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult SubscribeEmail(NewsLetter newsLetter)
        {
            _context.NewsLetters.Add(newsLetter);
            _context.SaveChanges();
            TempData["ToastType"] = "success";
            TempData["Abonelik"] = "Bültenimize başarıyla abone oldunuz!";
            return RedirectToAction("Index");
        }
    }
}
