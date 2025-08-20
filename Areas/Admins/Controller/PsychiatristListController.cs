using Microsoft.AspNetCore.Mvc;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    public class PsychiatristListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
