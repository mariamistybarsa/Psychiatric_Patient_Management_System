using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class ReviewsController : Controller
    {
        private readonly DapperContext _context;
        public ReviewsController(DapperContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Reviews()
        {
            // Get logged-in psychiatrist Id from session
            int psychiatristId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            var medParams = new DynamicParameters();
            medParams.Add("@flag", 2); // 2 = fetch reviews
            medParams.Add("@PsychiatristId", psychiatristId);

            using var connection = _context.CreateConnection();

            var reviews = await connection.QueryAsync<ReviewVm>(
                "Sp_Review",
                medParams,
                commandType: CommandType.StoredProcedure
            );

            return View(reviews.ToList());
        }
    }
    }
