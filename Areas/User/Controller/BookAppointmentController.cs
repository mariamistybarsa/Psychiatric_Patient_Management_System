
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                parameters.Add("@flag", 5); // flag 5 gets all bookings for the user
                parameters.Add("@UserId", userId); // current logged-in user

                var data = connection.Query<BookingHistoryVM>(
                    "Sp_BookAppointment",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).ToList();

                // Optional: set a flag for pending notifications
                ViewBag.PendingCount = data.Count(b => b.ApprovalStatus == "Pending");

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
                        var times = GetHourlyTimes(res.StartTime, res.Endtime,data.PsychiatristId,data.AppointmentDate,_context);
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



        static List<string> GetHourlyTimes(string startTimeStr, string endTimeStr,int? userId,DateTime? appDate,DapperContext context)
        {
            List<string> timeList = new List<string>();
            DateTime start, end;

            string[] formats = { "H:mm", "HH:mm", "h:mmtt", "hh:mmtt" }; // supports 24h and 12h with AM/PM

            if (!DateTime.TryParseExact(startTimeStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
                throw new FormatException($"Invalid start time: {startTimeStr}");

            if (!DateTime.TryParseExact(endTimeStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
                throw new FormatException($"Invalid end time: {endTimeStr}");
            var parameters = new DynamicParameters();
            parameters.Add("@@PsychiatristId", userId);
            parameters.Add("@Flag", 12);
            parameters.Add("@AppointmentDate", appDate);
            // add other parameters as needed

            var bookedTimeStrings  = context.CreateConnection().Query<dynamic>(
         "Sp_BookAppointment",
         parameters,
         commandType: CommandType.StoredProcedure
     ).ToList();
            var bookedTimes = bookedTimeStrings
        .Select(t => DateTime.ParseExact(t.AppointmentTime, "h.mmtt", CultureInfo.InvariantCulture))
        .ToList();

            while (start <= end)
            {

                if (!bookedTimes.Any(t => t.TimeOfDay == start.TimeOfDay))
                {
                    timeList.Add(start.ToString("HH:mm")); // add only if available
                }
                start = start.AddHours(1);
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
                parameters.Add("@flag", 6); 
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



        //Payment
        public IActionResult Payment()
        {

            return View();

        }

    }
}