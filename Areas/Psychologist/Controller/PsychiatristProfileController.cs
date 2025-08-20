using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class PsychiatristProfileController : Controller
    {
        private readonly DapperContext _context;
        public PsychiatristProfileController(DapperContext context)
        {
            _context = context;
        }

        
        public IActionResult profile()
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 2); 
                var data = connection.Query<ProfilePsychiatrist>(
                    "Sp_ProfilePsychiatrist",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return View(data);
            }
        }

       
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(ProfilePsychiatrist p)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1); // Insert flag
                    parameters.Add("@ProfileId", p.@ProfileId);
                    parameters.Add("@Bio", p.Bio);
                    parameters.Add("@Specialization", p.Specialization);
                    parameters.Add("@Experience", p.Experience);
                    parameters.Add("@EmergencyContact", p.EmergencyContact);

                    connection.Execute("Sp_ProfilePsychiatrist", parameters, commandType: CommandType.StoredProcedure);
                }

                return Json(new { success = true, redirectUrl = Url.Action("profile") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
