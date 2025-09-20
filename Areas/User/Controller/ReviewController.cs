using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.User.Controllers
{
    [Area("User")]
    public class ReviewController : Controller
    {
        public readonly DapperContext _context;
        public ReviewController(DapperContext context)
        {
            _context = context;
        }
        public IActionResult ReviewtoPsychiatrist(int bookingId)
        {
            if (bookingId == 0)
                return BadRequest("BookingId is required");

            ViewBag.BookingId = bookingId;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ReviewtoPsychiatrist(ReviewVm r)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@flag", 1);
                parameters.Add("@BookingId", r.BookingId);
                parameters.Add("@Rate", r.Rate);
                parameters.Add("@Comment", r.Comment);

                await connection.ExecuteAsync(
              "Sp_Review",
              parameters,
              commandType: CommandType.StoredProcedure
          );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}

