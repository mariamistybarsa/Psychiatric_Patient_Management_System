using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class AppointmentController : Controller
    {

        private readonly DapperContext _context;


        public AppointmentController(DapperContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 2);
                    var data = connection.Query<AppointmentPsychiatrist>(

                      "Sp_AppointmentPsychiatrist",
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
        public IActionResult Form()
        {
            return View();


        }


        [HttpPost]
        public IActionResult Save(AppointmentPsychiatrist m)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Please fill all required fields." });

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1);
                    parameters.Add("@PsyAppId", m.PsyAppId);
                    parameters.Add("@UserId", m.UserId);
                    parameters.Add("@Startday", m.Startday);
                    parameters.Add("@Endday", m.Endday);
                    parameters.Add("@StartTime", m.StartTime);
                    parameters.Add("@EndTime", m.EndTime);

                    connection.Execute(
                        "Sp_AppointmentPsychiatrist",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                return Json(new { success = true, message = "Appointment added successfully!", redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult getSchedule()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SceduleForm()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 13);
                    var psychiatrists = connection.Query<dynamic>(

                      "SP_User",
                      parameters,
                      commandType: CommandType.StoredProcedure
                  );
                    ViewBag.Psychiatrists = psychiatrists;


                }

                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
            
           
        }


    }
}