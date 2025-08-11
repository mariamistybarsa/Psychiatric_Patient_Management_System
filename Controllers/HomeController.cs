using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Psychiatrist_Management_System.Data.Migrations;
using Psychiatrist_Management_System.Models;
using System.Diagnostics;

namespace Psychiatrist_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EFDBContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public HomeController(ILogger<HomeController> logger, EFDBContext context)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(User data)
        {
            var chkUser = _context.Users.FirstOrDefault(x => x.Email == data.Email);
            if (chkUser != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(chkUser, chkUser.Password, data.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    // Session এ Email বা UserName রাখো
                    HttpContext.Session.SetString("UserEmail", chkUser.Email);
                    HttpContext.Session.SetString("UserName", chkUser.UserName ?? "User");

                    if (chkUser.UsertypeId == 1)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "Admins" });
                    }
                    else if (chkUser.UsertypeId == 3)
                    {
                        return RedirectToAction("Index", "Dashboard", new { area = "User" });
                    }

                    return RedirectToAction("Index", "Dashboard", new { area = "Psychologist" });
                }
            }
            ViewData["LoginError"] = "Invalid Email or Password";
            return View();
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.UserTypes = new SelectList(_context.UserTypes.ToList(), "UserTypeId", "UserTypeName");
            return View();
        }

        // POST: Register
        [HttpPost]
        public IActionResult Register(User data)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == data.Email);
                if (existingUser != null)
                {
                    ViewBag.UserTypes = new SelectList(_context.UserTypes.ToList(), "UserTypeId", "UserTypeName");
                    ViewData["Error"] = "Email already exists.";
                    return View(data);
                }

                data.Password = _passwordHasher.HashPassword(data, data.Password);

                _context.Users.Add(data);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            ViewBag.UserTypes = new SelectList(_context.UserTypes.ToList(), "UserTypeId", "UserTypeName");
            return View(data);
        }



        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // সব Session ডেটা ক্লিয়ার করে
            return RedirectToAction("Index", "Home"); //
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult About()
        {
            return View(); // Looks for Views/Home/About.cshtml
        }
    }
}
