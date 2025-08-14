using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class PatientListController : Controller
    {
        private readonly DapperContext _context;
        public PatientListController(DapperContext context)
        {
            _context = context;
        }
        public IActionResult PatientInfo()
        {
            using (var connection = _context.CreateConnection())
            {
              
                var p = new DynamicParameters();
                p.Add("@flag", 8); // Get All
                var data = connection.Query<User>(
                    "Sp_User",
                    p,
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();

                return View(data); 
            }
        }




    }
}
