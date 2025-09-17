using Microsoft.AspNetCore.Mvc;

namespace Psychiatrist_Management_System.Areas.User.Controllers
{

    public class PrescriptionController : Controller
    {
        [Area("User")]
        public IActionResult PrescriptionList()
        {
            return View();
        }
    }
}
