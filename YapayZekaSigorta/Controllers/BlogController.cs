using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using YapayZekaSigorta.Context;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class BlogController : Controller
    {
        private readonly InsureContext _context;

        public BlogController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult BlogList()
        {
            return View();
        }
        public IActionResult GetBlogByCategory(int id)
        {
            ViewBag.c = id;
            return View();
        }
        [HttpGet]
        public PartialViewResult GetBlog()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult GetBlog(string keyword)
        {
            return View();
        }
        public IActionResult BlogDetail(int id)
        {
            ViewBag.i = id;
            TempData["id"] = id;
            return View();
        }
        [HttpGet]
        public PartialViewResult AddComment()
        {
            ViewBag.id = TempData["id"];
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.AppUserId = "d89dfbb7-5726-4a2b-802a-12efaa15c877";

            using (var client = new HttpClient())
            {
                var apiKey = "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";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                try
                {
                    var translateRequestBody = new
                    {
                        inputs = comment.CommentDetail
                    };

                    var translateJson = JsonSerializer.Serialize(translateRequestBody);
                    var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

                    var translateResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
                    var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

                    string englishText = comment.CommentDetail;
                    if (translateResponseString.TrimStart().StartsWith("["))
                    {
                        var translateDoc = JsonDocument.Parse(translateResponseString);
                        englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
                    }
                    //ViewBag.v = englishText;

                    var toxicRequestBody = new
                    {
                        inputs = englishText
                    };

                    var toxicJson = JsonSerializer.Serialize(toxicRequestBody);
                    var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
                    var toxicResponse = await client.PostAsync("https://router.huggingface.co/hf-inference/models/unitary/toxic-bert", toxicContent);
                    var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

                    if (toxicResponseString.TrimStart().StartsWith("["))
                    {
                        var toxicDoc = JsonDocument.Parse(toxicResponseString);
                        foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                        {
                            string label = item.GetProperty("label").GetString();
                            double score = item.GetProperty("score").GetDouble();

                            if (score > 0.5)
                            {
                                comment.CommentStatus = "Toksik Yorum";
                                break;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(comment.CommentStatus))
                    {
                        comment.CommentStatus = "Yorum Onaylandı";
                    }
                }
                catch (Exception)
                {

                    comment.CommentStatus = "Bir Hata Oluştu";
                }
            }
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("BlogList");
        }

    }
}
