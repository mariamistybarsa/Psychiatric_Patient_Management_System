using Microsoft.AspNetCore.Mvc;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
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
