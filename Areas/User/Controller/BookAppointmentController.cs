
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.User.Controllers
{
    [Area("User")]
    public class BookAppointmentController : Controller
    {
        public readonly DapperContext _context;
        public BookAppointmentController(DapperContext context)
        {
            _context = context;
        }
        public IActionResult BookFrontPage()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 2);
                    var data = connection.Query<PsyProfileVM>(

                      "Sp_BookAppointment",
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

        [HttpGet]
        public IActionResult Schedule(int PsychiatristId)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 3);
            parameters.Add("@PsychiatristId", PsychiatristId);

            var data = connection.Query<PsychiatristSchedule>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return View(data);
        }
    }
}