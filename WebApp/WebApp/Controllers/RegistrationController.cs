using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Models;
using System.Xml.Linq;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        public ChinaStockContext db;

        public RegistrationController(ChinaStockContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task Authenticate(string userPhone)
        {
            var claim = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, userPhone) };
            ClaimsIdentity id = new ClaimsIdentity(claim, "ApplicationCobokie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpGet]
        [Route("Registration")]
        public IActionResult Registration()
        {
            return View(new User());
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(User user)
        {
            user.RoleId = db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Пользователь")).Result.IdRole;
            Random random = new Random();
            user.Identical_Number = random.Next(11111, 99999).ToString();
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            if (HttpContext.Session.Keys.Contains("AuthUser") && HttpContext.Session.Keys.Contains("Admin"))
            {
                return RedirectToAction("AdminPage", "Home");
            }
            if (HttpContext.Session.Keys.Contains("AuthUser") && !HttpContext.Session.Keys.Contains("Admin"))
            {
                return RedirectToAction("UserPage", "Home");
            }
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(c => c.PhoneNumber.Equals(model.PhoneNumber) && c.Password.Equals(model.Password));
                if (user != null && user.RoleId.Equals(db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Пользователь")).Result.IdRole))
                {
                    HttpContext.Session.SetString("AuthUser", model.PhoneNumber);
                    await Authenticate(model.PhoneNumber);
                    return RedirectToAction("UserPage", "Home");
                }
                if (user != null && user.RoleId.Equals(db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Администратор")).Result.IdRole))
                {
                    HttpContext.Session.SetString("AuthUser", model.PhoneNumber);
                    HttpContext.Session.SetString("Admin", "True");
                    await Authenticate(model.PhoneNumber);
                    return RedirectToAction("AdminPage", "Home");
                }
            }
            return RedirectToAction("SignIn", "Home");
        }
    }
}
