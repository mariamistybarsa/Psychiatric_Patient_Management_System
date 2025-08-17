using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;
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

        public IActionResult Edit(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var p = new DynamicParameters();
                p.Add("@flag", 9); 
                p.Add("@Id", id);
                var data = connection.QueryFirstOrDefault<User>(
                    "Sp_User",
                    p,
                    commandType: System.Data.CommandType.StoredProcedure
                );
                return View(data);
            }
        }
        public IActionResult Delete(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 10); // Delete
                    parameters.Add("@UserId", id);
                    connection.Execute(
                       "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return RedirectToAction("PatientInfo");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Approve(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 11); // Delete
                    parameters.Add("@UserId", id);
                    connection.Execute(
                       "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return RedirectToAction("PatientInfo");
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}



