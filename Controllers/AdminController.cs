using Microsoft.AspNetCore.Mvc;

namespace CafeOrderingApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("CafeOrders", "Order");
        }
    }
}
