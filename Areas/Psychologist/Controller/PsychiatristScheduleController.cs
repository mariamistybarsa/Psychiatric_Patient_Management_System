//PsychiatristScheduleController


using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Threading.Tasks;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class PsychiatristScheduleController : Controller
    {
        public readonly DapperContext _context;

        public PsychiatristScheduleController(DapperContext context)
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
                    parameters.Add("@UserId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    var data = connection.Query<PsychiatristSchedule>(

                      "Sp_PsychiatristSchedule",
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
        public async Task<IActionResult> Form()
        {
            List<string> allDays = new List<string>
    {
        "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"
    };

            // Get psychiatrist's already booked schedules
            int id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            string sql = @"SELECT StartDay, EndDay FROM PsychiatristSchedule 
                   WHERE PsychiatristId = @Id AND Status = 'Available'";
            var schedules = (await _context.CreateConnection()
                                .QueryAsync<(string StartDay, string EndDay)>(sql, new { Id = id }))
                                .ToList();

            foreach (var schedule in schedules)
            {
                int startIndex = allDays.IndexOf(schedule.StartDay.Replace(" ",""));
                int endIndex = allDays.IndexOf(schedule.EndDay.Replace(" ",""));

                if (startIndex == -1 || endIndex == -1)
                    continue;

                List<string> bookedDays;

                if (startIndex <= endIndex)
                    bookedDays = allDays.GetRange(startIndex, (endIndex - startIndex) + 1);
                else
                    bookedDays = allDays.Skip(startIndex).Concat(allDays.Take(endIndex + 1)).ToList();

                allDays = allDays.Except(bookedDays).ToList(); // Remove booked days
            }

            ViewBag.AvailableDays = allDays;
            return View();
        }


        [HttpPost]
        public IActionResult Save(PsychiatristSchedule p)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();

                    // Use the same flag for insert or update
                    // If Id = 0 → insert, else → update
                    if (p.PsychiatristScheduleId == 0)
                    {
                        parameters.Add("@flag", 1); // Insert
                    }
                    else
                    {
                        parameters.Add("@flag", 5); // Update
                    }

                    parameters.Add("@PsychiatristScheduleId", p.PsychiatristScheduleId);
                    parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    parameters.Add("@StartDay", p.StartDay);
                    parameters.Add("@EndDay", p.EndDay);
                    parameters.Add("@StartTime", p.StartTime);
                    parameters.Add("@EndTime", p.EndTime);
                    parameters.Add("@Status", p.Status);

                    connection.Execute("Sp_PsychiatristSchedule", parameters, commandType: CommandType.StoredProcedure);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                List<string> allDays = new List<string>
        {
            "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"
        };
                ViewBag.AvailableDays = allDays;

                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 4); // fetch single record
                    parameters.Add("@PsychiatristScheduleId", id);

                    var schedule = connection.QuerySingleOrDefault<PsychiatristSchedule>(
                        "Sp_PsychiatristSchedule",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (schedule == null)
                        return NotFound();

                    return View(schedule); // Pass the schedule to pre-fill form
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
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
                    parameters.Add("@PsychiatristScheduleId", id);
                    connection.Execute("Sp_PsychiatristSchedule", parameters, commandType: CommandType.StoredProcedure);
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