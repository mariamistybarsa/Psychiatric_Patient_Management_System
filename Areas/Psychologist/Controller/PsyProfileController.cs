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
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 2);
                    var data = connection.Query<PsyProfileVM>(

                      "Sp_PsyProfileVM",
                      parameters,
                      commandType: CommandType.StoredProcedure
                  );


                    return View(data);
                }
            }
            catch (Exception)
            {
                throw;
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

    }
}