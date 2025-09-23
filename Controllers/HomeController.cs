using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Interface;
using Psychiatrist_Management_System.Models;
using System.Data;
using System.Diagnostics;

namespace Psychiatrist_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly DapperContext _context;
        private readonly IMail _mail;

        public HomeController(DapperContext context, IMail mail)
        {
            _context = context;
            _mail = mail;
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
        public IActionResult Login(UserVM model)
        {
            _mail.SendEmailAsync(model.Email, "Signup Successful", $"Your account has been created successfully. Your UserName : {model.Email} And password is: {model.Password}");
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 7);
                parameters.Add("@Email", model.Email);
                parameters.Add("@Password", model.Password);

                var data = connection.QueryFirstOrDefault<UserVM>(
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

                    if (data.UsertypeId == 3 && data.Status != "Approved")
                    {
                        return Json(new { success = false, message = "Your account is not approved yet." });
                    }
                    else
                    {
                        if (data.UsertypeId == 2 && data.Status != "Approved")
                        {
                            return Json(new { success = false, message = "Your account is not approved yet." });
                        }
                    }
                    HttpContext.Session.SetString("UserEmail", data.Email);
                    HttpContext.Session.SetString("UserName", data.UserName);
                    HttpContext.Session.SetString("UserId", data.UserId.ToString());
                    HttpContext.Session.SetString("UsertypeId", data.UsertypeId.ToString());

                    var redirecturl = "";

                    if (data.UsertypeId == 3)
                    {
                        redirecturl = Url.Action("Index", "Dashboard", new { area = "User" });
                    }
                    else if (data.UsertypeId == 2)
                    {
                        redirecturl = Url.Action("Index", "Dashboard", new { area = "Psychologist" });
                    }
                    else
                    {
                        redirecturl = Url.Action("Index", "Dashboard", new { area = "Admins" });
                    }

                    return Json(new { success = true, message = "Successful", redirectUrl = redirecturl, userInfo = data });
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
                    parameters = new DynamicParameters();
                    parameters.Add("@flag", 14); // UserTypes fetch (assuming flag 4 is for user types)
                    var userTypes = connection.Query<UserType>(
                        "Sp_Register",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                    ViewBag.UserTypeList = new SelectList(userTypes, "UserTypeId", "UserTypeName");

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(UserVM model)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var existingEmail = connection.QueryFirstOrDefault<UserVM>(
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }



    }
}













