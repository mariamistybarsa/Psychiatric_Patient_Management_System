using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Interface;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Threading.Tasks;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class AppointmentBookController : Controller
    {
        private readonly DapperContext _context;
        private readonly IMail _mailService;

        public AppointmentBookController(DapperContext context, IMail mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyBookings()
        {
            using var connection = _context.CreateConnection();
            var psychiatristUserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 11);
            parameters.Add("@PsychiatristId", psychiatristUserId);

            var bookings = connection.Query<BookingHistoryVM>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return View(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int BookingId, string Message)
        {
            using var connection = _context.CreateConnection();

            // Get booking details including patient email
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 16);
            parameters.Add("@BookingId", BookingId);

            var booking = connection.QueryFirstOrDefault<BookingHistoryVM>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (booking == null || string.IsNullOrEmpty(booking.UserEmail))
            {
                TempData["Error"] = "Patient email not found!";
                return RedirectToAction("MyBookings");
            }

            try
            {
                // Build email body
                string subject = $"Message Regarding Your Appointment (Booking ID: {booking.BookingId})";
                string body = $@"
            Dear {booking.PatientName},<br/><br/>
            You have received a message from your psychiatrist <b>{booking.PsychiatristName}</b> regarding your appointment.<br/><br/>
            <b>Booking Details:</b><br/>
            Booking ID: {booking.BookingId}<br/>
            Patient Name: {booking.PatientName}<br/>
            Psychiatrist: {booking.PsychiatristName}<br/>
            Appointment Date: {booking.AppointmentDate}<br/>
            Appointment Time: {booking.AppointmentTime}<br/>
            Appointment Day: {booking.AppointmentDay}<br/>
            BookingSerial: {booking.BookingSerial}<br/><br/>
            <b>Message from Psychiatrist:</b><br/>
            {Message}<br/><br/>
            Regards,<br/>
            {booking.PsychiatristName}
        ";

                await _mailService.SendEmailAsync(booking.UserEmail, subject, body);
                TempData["Success"] = "Message sent to patient successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error sending message: " + ex.Message;
            }

            return RedirectToAction("MyBookings");
        }

        public IActionResult Prescription()
        {
            return View();
        }



    }
}