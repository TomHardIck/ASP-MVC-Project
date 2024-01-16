using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdminController : Controller
    {
        private ChinaStockContext db;
        public AdminController(ChinaStockContext context)
        {
            db = context;
        }

        [Route("AdminPage")]
        public IActionResult AdminPage()
        {
            if (HttpContext.Session.GetString("AuthUser") != null && HttpContext.Session.Keys.Contains("Admin"))
            {
                List<Package> packages = new List<Package>();
                packages = db.Packages.ToList();
                return View(packages);
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }

        [HttpGet]
        [Route("EditStatus")]
        public IActionResult EditStatus(int id)
        {
            if (HttpContext.Session.GetString("AuthUser") != null && HttpContext.Session.Keys.Contains("Admin"))
            {
                Package package = db.Packages.Where(x => x.IdPackage == id).FirstOrDefault();
                return PartialView("_ConfirmAction", package);
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }


        [HttpPost]
        [Route("EditStatus")]
        public async Task<IActionResult> EditStatus(Package package)
        {
            if (package != null)
            {
                package.IsFinished = true;
                db.Packages.Update(package);
                await db.SaveChangesAsync();
                TempData["message"] = "Успешно изменен статус посылки!";
                TempData["type"] = "Success";
                return RedirectToAction("AdminPage");
            }
            TempData["message"] = "Ошибка!";
            TempData["type"] = "Error";
            return RedirectToAction("AdminPage");
        }

        [HttpGet]
        public IActionResult SetCurrency()
        {
            if (HttpContext.Session.GetString("AuthUser") != null && HttpContext.Session.Keys.Contains("Admin"))
            {
                return View();
            }
            TempData["message"] = "Необходимо авторизоваться в системе!";
            TempData["type"] = "Error";
            return RedirectToAction("SignIn", "Registration");
        }
    }
}
