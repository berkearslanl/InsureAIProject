using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class ArticleController : Controller
    {
        private readonly InsureContext _context;

        public ArticleController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult ArticleList()
        {
            var values = _context.Articles.Include(x=>x.AppUser).Include(x=>x.Category).ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddArticle()
        {
            var categories = _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            ViewBag.Categories = categories;


            var author = _context.Users
                .Select(x => new SelectListItem
                {
                    Text = x.Name + " " + x.Surname,
                    Value = x.Id
                }).ToList();

            ViewBag.Author = author;

            return View();
        }
        [HttpPost]
        public IActionResult AddArticle(Article article)
        {
            article.CreatedDate = DateTime.Now;
            _context.Articles.Add(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }
        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            var categories = _context.Categories
                .Select(x => new SelectListItem
                {
                    Text = x.CategoryName,
                    Value = x.CategoryId.ToString()
                }).ToList();

            ViewBag.Categories = categories;


            var author = _context.Users
                .Select(x => new SelectListItem
                {
                    Text = x.Name+" "+x.Surname,
                    Value = x.Id
                }).ToList();

            ViewBag.Author = author;

            var values = _context.Articles.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            var values = _context.Articles.FirstOrDefault(x => x.ArticleId == article.ArticleId);

            values.CreatedDate = article.CreatedDate;
            values.Title= article.Title;
            values.Content = article.Content;
            values.CategoryId = article.CategoryId;
            values.CoverImageUrl = article.CoverImageUrl;
            values.MainCoverImageUrl = article.MainCoverImageUrl;

            //_context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }
        public IActionResult DeleteArticle(int id)
        {
            var value = _context.Articles.Find(id);
            _context.Articles.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var apikey = "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";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);

            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role="system",content="Sen bir sigorta şirketi için çalışan, içerik yazarlığı yapan bir yapay zekasın. Kullanıcının verdsiği özet ve anahtar kelimelere göre, sigortacılık sektörüyle ilgili makale üret. En az 1000 karakter olsun."},
                    new{role="user",content=prompt}
                },
                temperature = 0.7
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.article = content;
            }
            else
            {
                ViewBag.article = "Bir hata oluştu! --> " + response.StatusCode;
            }
            return View();

        }

        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }
        }
        public class Choice
        {
            public Message message { get; set; }
        }
        public class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

    }
}
//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