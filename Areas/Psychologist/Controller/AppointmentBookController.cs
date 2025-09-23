//AppointmentBookController 


using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Interface;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<IActionResult> SendMessage(int BookingId, string Message, string AppointmentTime)
        {
            using var connection = _context.CreateConnection();

            // Get booking details
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
                string subject = $"Message Regarding Your Appointment (Booking ID: {booking.BookingId})";
                string body = $@"
Dear {booking.PatientName},<br/><br/>
You have received a message from your psychiatrist <b>{booking.PsychiatristName}</b> regarding your appointment.<br/><br/>
<b>Booking Details:</b><br/>
Booking ID: {booking.BookingId}<br/>
Patient Name: {booking.PatientName}<br/>
Psychiatrist: {booking.PsychiatristName}<br/>
Appointment Date: {booking.AppointmentDate.ToShortDateString()}<br/>
Appointment Time: {AppointmentTime}<br/>  <!-- Use selected time here -->
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


        public async Task<IActionResult> Prescription(int bookingId)
{
    var medParams = new DynamicParameters();
    medParams.Add("@flag", 1);

    var medicine = await _context.CreateConnection().QueryAsync<dynamic>(
        "Sp_NewMedicine",
        medParams,
        commandType: CommandType.StoredProcedure
    );

    var presParams = new DynamicParameters();
    presParams.Add("@flag", 3);
    presParams.Add("@BookingId", bookingId);

    var prescriptionInfo = await _context.CreateConnection().QueryAsync<PrescriptionVM>(
        "Sp_Prescription",
        presParams,
        commandType: CommandType.StoredProcedure
    );

    ViewBag.Medicine = medicine;
    ViewBag.BookingId = bookingId;

    // Model হিসাবে পাঠানো হচ্ছে
    return View(prescriptionInfo.ToList());
}



        [HttpPost]
        public IActionResult SavePrescription([FromBody] List<PrescriptionVM> prescription)
        {
            using var connection = _context.CreateConnection();

            try
            {
                // ✅ Insert Prescription
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 1);
                parameters.Add("@PrescriptionId", prescription.First().PrescriptionId);
                parameters.Add("@BookingId", prescription.First().BookingId); // dynamic
                parameters.Add("@Advice", prescription.First().Advice);
                parameters.Add("@CreatedBy", "DoctorName"); // change as needed
                parameters.Add("@CreatedAt", DateTime.Now);

                // Assuming SP returns newly inserted PrescriptionId
                var booking = connection.QueryFirstOrDefault<PrescriptionVM>(
               "Sp_Prescription",
               parameters,
               commandType: CommandType.StoredProcedure
           );
                foreach(var item in prescription)
                {   // ✅ Insert Medicine related to that PrescriptionId
                    var parameters2 = new DynamicParameters();
                    parameters2.Add("@flag", 2);
                    //  parameters2.Add("@MedicinePrescriptionId", prescription.MedicinePrescriptionId);
                    parameters2.Add("@PrescriptionId", booking?.PrescriptionId);
                    parameters2.Add("@MedicineId", item.MedicineId);

                    // if 0, means new medicine
                    parameters2.Add("@MedicineDose", item.MedicineDose);

                    parameters2.Add("@Diagnosed", item.Diagnosed);



                    parameters2.Add("@MedicineDuration", item.MedicineDuration);
                    parameters2.Add("@Frequency", item.Frequency);
                    parameters2.Add("@Medicine_Notes", item.Medicine_Notes);

                    connection.Execute(
                        "Sp_Medicine",
                        parameters2,
                        commandType: CommandType.StoredProcedure
                    );
                }

             

                return Json(new { message = "Saved successfully",  });
            }
            catch (Exception ex)
            {
                return Json(new { message = "Error: " + ex.Message });
            }
        }
        public IActionResult MarkCompleted(int id)
        {
            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 18); // Our new flag
            parameters.Add("@BookingId", id);

            connection.Execute("Sp_BookAppointment", parameters, commandType: CommandType.StoredProcedure);

       
            // Redirect back to the booking list or same page
            return RedirectToAction("MyBookings"); // or your listing action
        }
    }
}
