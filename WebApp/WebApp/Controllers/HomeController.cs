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

        [Route("UserPage")]
        public IActionResult UserPage()
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                return View();
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }


        [HttpGet]
        [Route("AddPackage")]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                Package package = new Package();
                return PartialView("_AddPackage", package);
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }


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
                TempData["message"] = "Посылка успешно добавлена!";
                TempData["type"] = "Success";
                return RedirectToAction("PackagesWindow");
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }


        [HttpGet]
        public IActionResult EditPackage(int id)
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                var packageToUpdate = db.Packages.Where(x => x.IdPackage.Equals(id)).FirstOrDefault();
                return PartialView("EditPackage", packageToUpdate);
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
            
        }


        [HttpPost]
        public async Task<IActionResult> EditPackage(Package package)
        {
            if (HttpContext.Session.GetString("AuthUser") != null)
            {
                db.Packages.Update(package);
                await db.SaveChangesAsync();
                TempData["message"] = "Посылка успешно изменена!";
                TempData["type"] = "Success";
                return RedirectToAction("PackagesWindow");
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }



        [HttpGet]
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
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }
    }
}