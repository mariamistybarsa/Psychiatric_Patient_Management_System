using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    
    public class ManageUserController : Controller
    {
        private readonly DapperContext _context;
        public ManageUserController(DapperContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1); // Get All

                    var data = connection.Query<UserType>(
                        "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return View(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public IActionResult Form()
        {
            return View();


        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3); // Assuming flag 3 means GetById in your stored procedure
                    parameters.Add("@UserTypeId", id);

                    var userType = connection.QuerySingleOrDefault<UserType>(
                        "Sp_User", parameters, commandType: CommandType.StoredProcedure);

                    if (userType == null)
                        return NotFound();

                    return View(userType);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpPost]
        public IActionResult Save(UserType u)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new { success = false, message = "Invalid data" });
            //}
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    if(u.UserTypeId == 0)
                    {
                        parameters.Add("@flag", 2); // Insert
                    }
                    else
                    {
                        parameters.Add("@flag", 4); // Insert
                        parameters.Add("@UserTypeId", u.UserTypeId); // Insert
                    }
                  
                    parameters.Add("@UserTypeName", u.UserTypeName);
                    connection.Execute(
                         "Sp_User",
                         parameters,
               commandType: CommandType.StoredProcedure
 );

                }
                return Json(new { success = true, message = "User Type saved successfully" })
                    ;
            }

            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3);         // Get By Id
                    parameters.Add("@UserTypeId", id);  // Use the id passed to method

                    var data = connection.QueryFirstOrDefault<UserType>(
                        "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (data == null)
                        return NotFound();

                    return View("Form" ,new UserType {UserTypeId = data.UserTypeId,UserTypeName = data.UserTypeName });  // Return Edit view with data
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public IActionResult Edit(UserType u)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", u);
            }

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 5);              // Update
                    parameters.Add("@UserTypeId", u.UserTypeId);  // Pass UserTypeId
                    parameters.Add("@UserTypeName", u.UserTypeName);

                    connection.Execute(
                        "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
             public IActionResult Delete(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 6); // Delete
                    parameters.Add("@UserTypeId", id);
                    connection.Execute(
                       "Sp_User",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
    }
