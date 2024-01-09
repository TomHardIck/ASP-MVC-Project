using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using System.Security.Cryptography;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        private ChinaStockContext db;


        public HomeController(ChinaStockContext context)
        {
            db = context;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //private async Task Authenticate(string userPhone)
        //{
        //    var claim = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, userPhone) };
        //    ClaimsIdentity id = new ClaimsIdentity(claim, "ApplicationCobokie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}

        //[HttpGet]
        //[Route("Registration")]
        //public IActionResult Registration()
        //{
        //    return View(new User());
        //}

        //[HttpPost]
        //[Route("Registration")]
        //public async Task<IActionResult> Registration(User user)
        //{
        //    user.RoleId = db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Пользователь")).Result.IdRole;
        //    string passwordToHash = user.Password;
        //    db.Users.Add(user);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("SignIn");
        //}

        //[HttpGet]
        //[Route("SignIn")]
        //public IActionResult SignIn()
        //{
        //    if (HttpContext.Session.Keys.Contains("AuthUser") && HttpContext.Session.Keys.Contains("Admin"))
        //    {
        //        return RedirectToAction("AdminPage", "Home");
        //    }
        //    if (HttpContext.Session.Keys.Contains("AuthUser") && !HttpContext.Session.Keys.Contains("Admin"))
        //    {
        //        return RedirectToAction("UserPage", "Home");
        //    }
        //    return View(new LoginModel());
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("SignIn")]
        //public async Task<IActionResult> SignIn(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = await db.Users.FirstOrDefaultAsync(c => c.PhoneNumber.Equals(model.PhoneNumber) && c.Password.Equals(model.Password));
        //        if (user != null && user.RoleId.Equals(db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Пользователь")).Result.IdRole))
        //        {
        //            HttpContext.Session.SetString("AuthUser", model.PhoneNumber);
        //            await Authenticate(model.PhoneNumber);
        //            return RedirectToAction("UserPage", "Home");
        //        }
        //        if (user != null && user.RoleId.Equals(db.UserRoles.FirstOrDefaultAsync(x => x.RoleName.Equals("Администратор")).Result.IdRole))
        //        {
        //            HttpContext.Session.SetString("AuthUser", model.PhoneNumber);
        //            HttpContext.Session.SetString("Admin", "True");
        //            await Authenticate(model.PhoneNumber);
        //            return RedirectToAction("AdminPage", "Home");
        //        }
        //    }
        //    return RedirectToAction("SignIn", "Home");
        //}


        [Authorize]
        [Route("UserPage")]
        public IActionResult UserPage()
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                return View();
            }
            return RedirectToAction("SignIn", "Registration");
        }



        [Authorize]
        [Route("AdminPage")]
        public IActionResult AdminPage()
        {
            if (HttpContext.Session.GetString("AuthUser") != null && HttpContext.Session.Keys.Contains("Admin"))
            {
                return View();
            }
            return RedirectToAction("SignIn", "Registration");
        }


        [Authorize]
        [HttpGet]
        [Route("AddPackage")]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                Package package = new Package();
                return PartialView("_AddPackage", package);
            }
            return RedirectToAction("SignIn", "Registration");
        }


        [Authorize]
        [HttpPost]
        [Route("AddPackage")]
        public async Task<IActionResult> Create(Package package)
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                Random random = new Random();
                package.IsFinished = false;
                db.Packages.Add(package);
                await db.SaveChangesAsync();
                List<Package> packages = db.Packages.ToList();
                db.PackagesOfUsers.Add(new PackagesOfUser { PackageId = packages.Last().IdPackage, UserId = db.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(HttpContext.Session.GetString("AuthUser").ToString())).Result.IdUser, IdenticalNumber = 'A' + random.Next(10000000, 99999999).ToString() });
                await db.SaveChangesAsync();
                return RedirectToAction("PackagesWindow");
            }
            return RedirectToAction("SignIn", "Registration");
        }


        [Authorize]
        [HttpGet]
        public IActionResult EditPackage(int id)
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                var packageToUpdate = db.Packages.Where(x => x.IdPackage.Equals(id)).FirstOrDefault();
                return PartialView("EditPackage", packageToUpdate);
            }
            return RedirectToAction("SignIn", "Registration");
            
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPackage(Package package)
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                db.Packages.Update(package);
                await db.SaveChangesAsync();
                return RedirectToAction("PackagesWindow");
            }
            return RedirectToAction("SignIn", "Registration");
        }



        [Authorize]
        [Route("MyPackages")]
        public IActionResult PackagesWindow()
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                List<Package> packages = new List<Package>();
                var queryList = db.PackagesOfUsers.ToList();
                var allUsers = db.Users.ToList();
                foreach (var elem in queryList)
                {
                    if (elem.UserId.Equals(allUsers.FirstOrDefault(x => x.PhoneNumber.Equals(HttpContext.Session.GetString("AuthUser"))).IdUser))
                    {
                        packages.Add(db.Packages.FirstOrDefault(x => x.IdPackage.Equals(elem.PackageId)));
                    }
                }
                return View(packages);
            }
            return RedirectToAction("SignIn", "Registration");
        }


        [Authorize]
        [HttpPost]
        [Route("MyPackages")]
        public async Task<IActionResult> PackagesWindow(Package package)
        {
            if(HttpContext.Session.GetString("AuthUser") != null)
            {
                Random random = new Random();
                package.IsFinished = false;
                db.Packages.Add(package);
                await db.SaveChangesAsync();
                List<Package> packages = db.Packages.ToList();
                db.PackagesOfUsers.Add(new PackagesOfUser { PackageId = packages.Last().IdPackage, UserId = db.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Equals(HttpContext.Session.GetString("AuthUser").ToString())).Result.IdUser, IdenticalNumber = 'A' + random.Next(10000000, 99999999).ToString() });
                await db.SaveChangesAsync();
                return RedirectToAction("PackagesWindow");
            }
            return RedirectToAction("SignIn", "Registration");
        }
    }
}