using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class DashboardController : Controller
    {
        private readonly DapperContext _context;

        public DashboardController(DapperContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int psychiatristId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            using (var conn = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PsychiatristId", psychiatristId);

                // Flag 1: Summary
                parameters.Add("@flag", 1);
                var summary = (await conn.QueryAsync<PsychiatristDashboardVM>(
                    "Sp_PsychiatristDashboard",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure)).FirstOrDefault();

                // Flag 2: Patients + Reviews
                parameters.Add("@flag", 2); // reuse same parameters object
                var patients = (await conn.QueryAsync<PsychiatristDashboardVM>(
                    "Sp_PsychiatristDashboard",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure)).ToList();

                ViewBag.Summary = summary;
                ViewBag.Patients = patients;
            }

            return View();
        }
    }
}


//namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
//{
//    [Area("Psychologist")]
//    public class DashboardController : Controller
//    {
//        private readonly DapperContext _context;

//        public DashboardController(DapperContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            int psychiatristId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

//            using (var conn = _context.CreateConnection())
//            {
//                var parameters = new DynamicParameters();
//                parameters.Add("@PsychiatristId", psychiatristId);
//                parameters.Add("@flag", 1);

//                var result = await conn.QueryAsync<dynamic>(
//                    "Sp_PsychiatristDashboard",
//                    parameters,
//                    commandType: System.Data.CommandType.StoredProcedure);

//                ViewBag.DashboardData = result;
//            }

//            return View();
//        }
//    }
//}
