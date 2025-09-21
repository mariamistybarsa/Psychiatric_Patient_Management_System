using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Authorization.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class DashboardController : Controller
    {
        private readonly DapperContext _context;
        public DashboardController(DapperContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using var conn = _context.CreateConnection();
            var data = await conn.QueryAsync<dynamic>(
                "Sp_AdminBookingSummary",
                commandType: CommandType.StoredProcedure
            );

            // Render normal table
            return View(data);
        }
        

        
        [HttpGet]
        public async Task<IActionResult> GetBookingSummary()
        {
            using var conn = _context.CreateConnection();
            var data = await conn.QueryAsync<dynamic>(
                "Sp_AdminBookingSummary",
                commandType: CommandType.StoredProcedure
            );

            // Render normal table
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingMonthly()
        {
            using var conn = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 1); // SP এর জন্য flag parameter

            var data = await conn.QueryAsync<GetMonthlySummary>(
                "Sp_AdminBookingSummary",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return View(data); // normal table view
        }
        [HttpGet]
        public async Task<IActionResult> GetBookingMonthlys(int PsychiatristId)
        {
            using var conn = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@flag", 2); // SP এর জন্য flag parameter
            parameters.Add("@PsychiatristId", PsychiatristId); // SP এর জন্য PsychiatristId parameter

            var data = await conn.QueryAsync<GetMonthlySummary>(
                "Sp_AdminBookingSummary",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return View(data); // normal table view
        }



    }
}
