using Microsoft.AspNetCore.Mvc;

namespace Authorization.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
