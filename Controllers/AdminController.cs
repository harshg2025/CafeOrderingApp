using Microsoft.AspNetCore.Mvc;

namespace CafeOrderingApp.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Setup simple secure admin credentials
            if (username == "Harsh" && password == "Harsh123")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("UserName", "Harsh");
                return RedirectToAction("Index");
            }
            
            ViewBag.Error = "Invalid Admin ID or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            // Protect Dashboard View
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
