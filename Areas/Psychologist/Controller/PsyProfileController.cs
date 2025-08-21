using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class PsyProfileController : Controller
    {
        public readonly DapperContext _context;

        public PsyProfileController(DapperContext context)
        {
            _context = context;
        }

      public IActionResult Index()
{
    using (var connection = _context.CreateConnection())
    {
        var parameters = new DynamicParameters();
        parameters.Add("@flag", 2); // Get profile for current user
        parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));

        var profile = connection.Query<PsyProfileVM>(
            "Sp_PsyProfileVM",
            parameters,
            commandType: CommandType.StoredProcedure
        ).FirstOrDefault(); // ✅ Single profile

        return View(profile);
    }
}
        public IActionResult Form()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Save(PsyProfileVM p)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@flag", 1);
                    parameters.Add("@ProfileId", p.ProfileId);
                    parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    parameters.Add("@Bio", p.Bio);
                    parameters.Add("@Specialization", p.Specialization);
                    parameters.Add("@Experience", p.Experience);
                    parameters.Add("@EmergencyContact", p.EmergencyContact);
                    parameters.Add("@Imageurl", p.Imageurl);

                    connection.Execute("Sp_PsyProfileVM", parameters, commandType: CommandType.StoredProcedure);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3); // Delete
                    parameters.Add("@PsychiatristId", id);
                    connection.Execute("Sp_PsyProfileVM", parameters, commandType: CommandType.StoredProcedure);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }
        }

    }
}