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
                p.Add("@flag", 8); 
                var data = connection.Query<UserVM>(
                    "Sp_User",
                    p,
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();

                return View(data);
            }
        }
        public IActionResult Psychiatrist()
        {
            using (var connection = _context.CreateConnection())
            {

                var p = new DynamicParameters();
                p.Add("@flag", 13); // Get All
                var data = connection.Query<UserVM>(
                    "Sp_User",
                    p,
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();

                return View(data);
            }
        }

        // GET: Edit page
        public IActionResult Edit(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var p = new DynamicParameters();
                p.Add("@flag", 9); // Fetch single user
                p.Add("@UserId", id); // Correct parameter name
                var data = connection.QueryFirstOrDefault<UserVM>(
                    "Sp_User",
                    p,
                    commandType: CommandType.StoredProcedure
                );
                return View(data);
            }
        }

        // POST: Update user
        [HttpPost]
        
        public IActionResult Edit(UserVM model)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@flag", 14);
                    p.Add("@UserId", model.UserId);
                    p.Add("@UserName", model.UserName);
                    p.Add("@Email", model.Email);
                    p.Add("@PhoneNumber", model.PhoneNumber);
                    p.Add("@DesignationName", model.DesignationName);
                    p.Add("@DesignationId", model.DesignationId);
                    p.Add("@Age", model.Age);
                    p.Add("@Address", model.Address);
                    p.Add("@BloodGroup", model.BloodGroup);

                    connection.Execute(
                        "Sp_User",
                        p,
                        commandType: CommandType.StoredProcedure
                    );
                }

                TempData["Message"] = "User updated successfully!";
                if(model.UsertypeId == 2)
                {
                    return RedirectToAction("Psychiatrist");
                }
                return RedirectToAction("PatientInfo");
            }
            catch (Exception ex)
            {
                // Optional: log the error
                TempData["Error"] = "An error occurred while updating the user: " + ex.Message;
                return View(model);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                int userTypeId;
                using (var connection = _context.CreateConnection())
                {
                    // First fetch the user type
                    userTypeId = connection.QueryFirstOrDefault<int>(
                        "SELECT UsertypeId FROM Users WHERE UserId = @UserId",
                        new { UserId = id }
                    );

                    // Now delete the user
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 10); // Delete
                    parameters.Add("@UserId", id);

                    connection.Execute(
                        "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                TempData["Message"] = "User deleted successfully!";

                if (userTypeId == 2)
                {
                    return RedirectToAction("Psychiatrist");
                }
                return RedirectToAction("PatientInfo");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the user.";
                return RedirectToAction("PatientInfo");
            }
        }


        //public IActionResult Delete(int id)
        //{
        //    try
        //    {
        //        using (var connection = _context.CreateConnection())
        //        {
        //            var parameters = new DynamicParameters();
        //            parameters.Add("@flag", 10); // Delete
        //            parameters.Add("@UserId", id);
        //            connection.Execute(
        //               "Sp_User",
        //                parameters,
        //                commandType: CommandType.StoredProcedure
        //            );
        //        }
        //        return RedirectToAction("PatientInfo");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        public IActionResult Approve(int id)
        {
            try
            {
                int userTypeId;
                using (var connection = _context.CreateConnection())
                {
                    // Get the user type first (same idea as Delete)
                    userTypeId = connection.QueryFirstOrDefault<int>(
                        "SELECT UsertypeId FROM Users WHERE UserId = @UserId",
                        new { UserId = id }
                    );

                    // Now approve the user
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 11); // Approve
                    parameters.Add("@UserId", id);

                    connection.Execute(
                        "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                TempData["Message"] = "User approved successfully!";

                if (userTypeId == 2)
                {
                    return RedirectToAction("Psychiatrist");
                }
                return RedirectToAction("PatientInfo");
            }
            catch (Exception)
            {
                TempData["Error"] = "An error occurred while approving the user.";
                return RedirectToAction("PatientInfo");
            }
        }








        //public IActionResult Approve(int id)
        //{
        //    try
        //    {
        //        using (var connection = _context.CreateConnection())
        //        {
        //            var parameters = new DynamicParameters();
        //            parameters.Add("@flag", 11); // Delete
        //            parameters.Add("@UserId", id);
        //            connection.Execute(
        //               "Sp_User",
        //                parameters,
        //                commandType: CommandType.StoredProcedure
        //            );
        //        }
        //        return RedirectToAction("PatientInfo");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        [HttpGet]
 
        public IActionResult Search(string userName)
        {
            using (var connection = _context.CreateConnection())
            {
                // Jodi search empty hoy
                if (string.IsNullOrEmpty(userName))
                {
                    return Json(new { success = false, message = "Your data is not available" });
                }

                var p = new DynamicParameters();
                p.Add("@flag", 12); // search flag
                p.Add("@userName", userName);

                var data = connection.Query<UserVM>(
                    "Sp_User",
                    p,
                    commandType: System.Data.CommandType.StoredProcedure
                ).ToList();
         
                return View("PatientInfo", data);

            }
        }
    }
}


       



