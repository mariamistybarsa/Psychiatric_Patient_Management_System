
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

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
        public IActionResult Index()
        {
            try
            {
                // get current logged-in user id from session (or identity)
                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                using var connection = _context.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 5);
                parameters.Add("@UserId", userId); // pass current user to filter bookings

                var data = connection.Query<BookingHistoryVM>(
                    "Sp_BookAppointment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return View(data);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public IActionResult Book(int PsychiatristId)
        {
            ViewBag.PsychiatristId = PsychiatristId; // store in ViewBag
            return View();
        }

        [HttpPost]
        public IActionResult Book(BookVM b)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    // Use the same flag for insert or update
                    parameters.Add("@flag", 1);
                    parameters.Add("@PsychiatristId", b.PsychiatristId);
                    parameters.Add("@UserId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    parameters.Add("@AppointmentDate", b.AppointmentDate);
                    parameters.Add("@AppointmentTime", b.AppointmentTime);
                    parameters.Add("@AppointmentDay", b.AppointmentDay);
                    parameters.Add("@notes", b.notes);
                    var data = connection.Execute(
                        "Sp_BookAppointment",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetScheduleByAppointmentDate(GetScheduleByAppointmentDate data)
        {
            try
            {
                // Get the day name from the appointment date
                var dayName = data.AppointmentDate.Value.ToString("dddd", CultureInfo.InvariantCulture);

                // Example: call your stored procedure (replace "Sp_Something" with actual)
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PsychiatristId", data.PsychiatristId);
                    parameters.Add("@Flag", 4);

                    parameters.Add("@AppointmentDay", dayName);
                    // add other parameters as needed

                  var res= await connection.QueryFirstOrDefaultAsync(
                        "Sp_BookAppointment",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    if(res != null)
                    {
                        var times = GetHourlyTimes(res.StartTime, res.Endtime);
                        return Json(new { times = times, dayName = dayName });
                    }
                    return Json(null);                 
                }

            
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }




        }

       static List<string> GetHourlyTimes(string startTimeStr, string endTimeStr)
    {
        List<string> timeList = new List<string>();

        // Correct format for "8.00AM"
        DateTime start = DateTime.ParseExact(startTimeStr, "h.mmtt", CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(endTimeStr, "h.mmtt", CultureInfo.InvariantCulture);

        while (start <= end)
        {
            timeList.Add(start.ToString("h.mmtt")); // keeps same format
            start = start.AddHours(1); // hourly increment
        }

        return timeList;
    }
        [HttpPost]
        public IActionResult CancelBooking(int bookingId)
        {
            try
            {
                using var connection = _context.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 6); // Assuming flag 6 in your stored procedure handles cancellation
                parameters.Add("@BookingId", bookingId);

                connection.Execute(
                    "Sp_BookAppointment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                TempData["Message"] = "Booking cancelled successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error cancelling booking: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}