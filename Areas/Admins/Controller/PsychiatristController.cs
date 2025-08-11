using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class PsychiatristController : Controller
    {
        private readonly DapperContext _context;


        public PsychiatristController(DapperContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 2); // Get All

                    var data = connection.Query<PsychiatristVM>(
                        "Sp_Psychiatrist",
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

        public IActionResult Form()
        {
            return View();
        }

      

        [HttpPost]
        public IActionResult Save(PsychiatristVM model)
        {
            if (!ModelState.IsValid)
                return View("Form", model);

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 1); // Create
                                                // Usually no id for create, remove if not needed
                    parameters.Add("@name", model.name);
                    parameters.Add("@email", model.email);
                    parameters.Add("@specialization", model.specialization);

                    connection.Execute(
                        "Sp_Psychiatrist",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                }

                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Edit(int id) // GET Edit to load existing data
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 3); // Get By Id
                    parameters.Add("@id", id);

                    var data = connection.QueryFirstOrDefault<PsychiatristVM>(
                        "Sp_Psychiatrist",
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
        public IActionResult Edit(PsychiatristVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 4); // Update
                    parameters.Add("@id", model.id);
                    parameters.Add("@name", model.name);
                    parameters.Add("@email", model.email);
                    parameters.Add("@specialization", model.specialization);

                    connection.Execute(
                        "Sp_Psychiatrist",
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
                    parameters.Add("@flag", 5); // Delete
                    parameters.Add("@id", id);
                    connection.Execute(
                        "Sp_Psychiatrist",
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
