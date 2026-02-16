using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YapayZekaSigorta.Dtos;
using YapayZekaSigorta.Entities;

namespace YapayZekaSigorta.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRegisterDto p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            AppUser appUser = new AppUser()
            {
                Name = p.Name,
                Email = p.Email,
                Surname = p.Surname,
                UserName = p.Username,
                ImageUrl = "Test",
                Description = "Acıklama"
            };

            await _userManager.CreateAsync(appUser, p.Password);
            return RedirectToAction("UserLogin");
        }
        public IActionResult UserLogin()
        {
            return View();
        }
    }
}
