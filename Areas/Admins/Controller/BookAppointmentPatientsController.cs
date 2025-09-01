using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class BookAppointmentPatientsController : Controller
    {
        private readonly DapperContext _context;

        // Only one constructor
        public BookAppointmentPatientsController(DapperContext context)
        {
            _context = context;
        }

        // GET: Pending appointments
        public IActionResult AppointmentPatientLists()
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 7); // flag for pending bookings
            var data = connection.Query<BookingHistoryVM>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return View(data);
        }

        // POST: Approve booking
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveBooking(int bookingId)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 8); // flag for approval in SP
            parameters.Add("@BookingId", bookingId);
            connection.Execute("Sp_BookAppointment", parameters, commandType: CommandType.StoredProcedure);

            TempData["Message"] = "Booking approved. Please payment first.";
            return RedirectToAction("AppointmentPatientLists");
        }

        public IActionResult RejectBooking(int bookingId)
        {
            using var conn = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 9);
            parameters.Add("@BookingId", bookingId);
            conn.Execute("Sp_BookAppointment", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = "Booking rejected successfully.";
            return RedirectToAction("AppointmentPatientLists");
        }
        [HttpGet]
        public IActionResult AppointmentDetails(int userId)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 10);
            parameters.Add("@UserId", userId);

            var data = connection.Query<BookingHistoryVM>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();

            if (data == null || data.Count == 0)
            {
                TempData["Message"] = "No appointment details found for this user.";
                return RedirectToAction("PatientInfo");
            }

            return View(data);
        }

    }
}
   