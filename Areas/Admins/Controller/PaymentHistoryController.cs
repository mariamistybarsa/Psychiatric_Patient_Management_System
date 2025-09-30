using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]   
    public class PaymentHistoryController : Controller
    {
        private readonly DapperContext _context;
        public PaymentHistoryController(DapperContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Index( DateTime? startDate, DateTime? endDate)
        {
            try
            {
               
                 
                    // যদি StartDate null হয়, set মাসের প্রথম দিন
                    startDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    // যদি EndDate null হয়, set মাসের শেষ দিন
                    endDate ??= new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                            DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                    // 👉 Ensure EndDate includes whole day (23:59:59)
                    endDate = endDate.Value.Date.AddDays(1).AddTicks(-1);

                    ViewBag.StartDate = startDate;
                    ViewBag.EndDate = endDate;



                    using (var connection = _context.CreateConnection())
                {

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 2);
                    parameters.Add("@StartDate", startDate);
                    parameters.Add("@EndDate", endDate);
                    var data = connection.Query<PaymentVM>(

                      "Sp_Payment",
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


       


    }
}
