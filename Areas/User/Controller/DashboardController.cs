using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

[Area("User")]
public class DashboardController : Controller
{
    private readonly DapperContext _context;

    public DashboardController(DapperContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var name = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(name))
            return RedirectToAction("Login", "Account");

        ViewBag.UserName = name;

        var userIdStr = HttpContext.Session.GetString("UserId");
        int userId = Convert.ToInt32(userIdStr);



        List<BookingHistoryVM> bookings = new List<BookingHistoryVM>();

        using (var connection = _context.CreateConnection())
        {
            var parameters = new DynamicParameters();
            parameters.Add("@flag", 30);
            parameters.Add("@UserId", userId); // value only after null check

            bookings = connection.Query<BookingHistoryVM>(
                "Sp_BookAppointment",
                param: parameters,
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        return View(bookings);
    }
}
