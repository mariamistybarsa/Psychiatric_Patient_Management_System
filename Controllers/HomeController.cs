using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Diagnostics;

namespace Psychiatrist_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly DapperContext _context;
        public HomeController(DapperContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User model)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 7);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Password", model.Password);

                var data = connection.QueryFirstOrDefault<User>(
                    "Sp_User",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (data == null)
                {
                  
                    return Json(new { success = false, message = "Invalid Entry" });
                }
                else
                {
                    var redirecturl = "";
                    if (data.UsertypeId == 3)
                    {
                        redirecturl = Url.Action("Index", "Dashboard", new { area = "User" }); ;
                    }
                    return Json(new { success = true, message = "Successful" , redirectUrl = redirecturl });



                }



            }



        }






        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3); 
                    var data = connection.Query<DesinationVm>(
                        "Sp_Register",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    ViewBag.DesignationList = new SelectList(data, "DesignationId", "DesignationName");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register( User model)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    
                    var existingEmail = connection.QueryFirstOrDefault<User>(
                        "SELECT * FROM Users WHERE Email = @Email",
                        new { Email = model.Email }
                    );

                    if (existingEmail != null)
                    {
                        return Json(new { success = false, message = "Email already exists!" });
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1); // insert user
                    parameters.Add("@UserName", model.UserName);
                    parameters.Add("@Password", model.Password);
                    parameters.Add("@Email", model.Email);
                    parameters.Add("@UsertypeId", model.UsertypeId);
                    parameters.Add("@PhoneNumber", model.PhoneNumber);
                    parameters.Add("@DesignationId", model.DesignationId);
                    parameters.Add("@Age", model.Age);
                    parameters.Add("@Address", model.Address);
                    parameters.Add("@BloodGroup", model.BloodGroup);

                    connection.Execute("Sp_Register", parameters, commandType: CommandType.StoredProcedure);
                }

                return Json(new { success = true, redirectUrl = Url.Action("Login", "Home") });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
