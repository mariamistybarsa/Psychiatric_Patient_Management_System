using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class PsychiatristRecordController : Controller
    {
        private readonly DapperContext _context;

        public PsychiatristRecordController(DapperContext context)
        {
            _context = context;
        }

        // Show records for a given date range
        public IActionResult Record(DateTime? StartDate, DateTime? EndDate)
        {
            // যদি StartDate null হয়, set মাসের প্রথম দিন
            StartDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // যদি EndDate null হয়, set মাসের শেষ দিন
            EndDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            // 👉 Ensure EndDate includes whole day (23:59:59)
            EndDate = EndDate.Value.Date.AddDays(1).AddTicks(-1);

            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 1);
            parameters.Add("@StartDate", StartDate);
            parameters.Add("@EndDate", EndDate);

            var data = connection.Query<PsychiatristRecords>(
                "Sp_PsychiatristRecord",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            ViewBag.EndDate = EndDate;
            ViewBag.StartDate = StartDate;

            return View(data);
        }


        public IActionResult Details(int psychiatristId, DateTime? startDate, DateTime? endDate)
        {
            // Store psychiatristId in ViewBag (for hidden input)
            ViewBag.PsychiatristId = psychiatristId;
            // যদি StartDate null হয়, set মাসের প্রথম দিন
            startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // যদি EndDate null হয়, set মাসের শেষ দিন
            endDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            // 👉 Ensure EndDate includes whole day (23:59:59)
            endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            using var connection = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 2);
            parameters.Add("@PsychiatristId", psychiatristId);
            parameters.Add("@StartDate", startDate);
            parameters.Add("@EndDate", endDate);

            var data = connection.Query<PsychiatristRecords>(
                "Sp_PsychiatristRecord",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            ViewBag.PsychiatristName = data.FirstOrDefault()?.PsychiatristName;

            return View(data);
        }





    }
}