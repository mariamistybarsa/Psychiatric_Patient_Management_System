using Microsoft.AspNetCore.Mvc;

namespace Authorization.Areas.User.Controllers
{
    [Area("User")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var name = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(name))
                return RedirectToAction("Login", "Account"); // optional guard

            ViewBag.UserName = name; // ✅ push to ViewBag for the view
            return View();
        }
    }
}
