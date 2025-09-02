using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class AppointmentBookController : Controller
    {
        
        private readonly DapperContext _context;
  
        public AppointmentBookController(DapperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyBookings()
        {
            using var connection = _context.CreateConnection();
            var psychiatristUserId =Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 11);
            parameters.Add("@PsychiatristId", psychiatristUserId); // psychiatrist UserId

            var bookings = connection.Query<BookingHistoryVM>(
                "Sp_BookAppointment",
                parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();

            return View(bookings);
        }

    }
}

