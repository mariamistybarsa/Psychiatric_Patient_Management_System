using Dapper;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class DesignationController : Controller
    {
        private readonly DapperContext _context;
        public DesignationController(DapperContext context)
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
                    parameters.Add("@flag", 2);
                    var data = connection.Query<DesinationVm>(

                      "Sp_Designation",
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

        [HttpPost]
        public IActionResult Save(DesinationVm model)
        {

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1); // Insert
                    parameters.Add("@DesignationId", model.DesignationId);
                    parameters.Add("@DesignationName", model.DesignationName);


                    connection.Execute("Sp_Designation", parameters, commandType: CommandType.StoredProcedure);
                }
                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Form", model);
            }
        }
        public IActionResult Edit(int id)
        {

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3);
                    parameters.Add("DesignationId", id);
                    var data = connection.QueryFirstOrDefault<DesinationVm>(
                        "Sp_Designation",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (data == null)
                        return NotFound();

                    return View("Edit", data); // Use the same Form view for edit
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(DesinationVm model)
        {
            try
            {

                using(var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 4); // Update
                    parameters.Add("@DesignationId", model.DesignationId);
                    parameters.Add("@DesignationName", model.DesignationName);


                    connection.Execute(
                        "Sp_Designation",
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

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 5); // Delete
                    parameters.Add("@DesignationId", id);
                    connection.Execute("Sp_Designation", parameters, commandType: CommandType.StoredProcedure);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }
        }


    }








}