
//PsyProfileController
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Psychiatrist_Management_System.Data;
using Psychiatrist_Management_System.Models;
using System.Data;

namespace Psychiatrist_Management_System.Areas.Psychologist.Controllers
{
    [Area("Psychologist")]
    public class PsyProfileController : Controller
    {
        public readonly DapperContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PsyProfileController(DapperContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
{
    using (var connection = _context.CreateConnection())
    {
        var parameters = new DynamicParameters();
        parameters.Add("@flag", 2); // Get profile for current user
        parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));

        var profile = connection.Query<PsyProfileVM>(
            "Sp_PsyProfileVM",
            parameters,
            commandType: CommandType.StoredProcedure
        ).FirstOrDefault(); 

        return View(profile);
    }
}
        public IActionResult Form()
        {
            return View();
        }

        private string UploadedFile(PsyProfileVM model)
        {
            string uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/Psychiatrist");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }



        [HttpPost]
        public IActionResult Save(PsyProfileVM p)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string uniqueFileName = UploadedFile(p);
                    var parameters = new DynamicParameters();

                    parameters.Add("@flag", 1);
                    parameters.Add("@ProfileId", p.ProfileId);
                    parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    parameters.Add("@Bio", p.Bio);
                    parameters.Add("@Specialization", p.Specialization);
                    parameters.Add("@Experience", p.Experience);
                    parameters.Add("@EmergencyContact", p.EmergencyContact);
                    parameters.Add("@Imageurl", uniqueFileName);

                    connection.Execute("Sp_PsyProfileVM", parameters, commandType: CommandType.StoredProcedure);
                }

                return RedirectToAction("Index") ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@flag", 4); // Get profile by ID for editing
                parameters.Add("@ProfileId", 0);
                parameters.Add("@PsychiatristId", id);

                var profile = connection.Query<PsyProfileVM>(
                    "Sp_PsyProfileVM",
                    parameters,
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();

                if (profile == null)
                {
                    return NotFound();
                }

                return View(profile);
            }
        }

        [HttpPost]
        public IActionResult Edit(PsyProfileVM p)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string uniqueFileName = UploadedFile(p);

                    var parameters = new DynamicParameters();
                    parameters.Add("@flag", 5); // Update profile
                    parameters.Add("@ProfileId", p.ProfileId);
                    parameters.Add("@PsychiatristId", Convert.ToInt32(HttpContext.Session.GetString("UserId")));
                    parameters.Add("@Bio", p.Bio);
                    parameters.Add("@Specialization", p.Specialization);
                    parameters.Add("@Experience", p.Experience);
                    parameters.Add("@EmergencyContact", p.EmergencyContact);
                    parameters.Add("@Imageurl", string.IsNullOrEmpty(uniqueFileName) ? p.Imageurl : uniqueFileName);

                    connection.Execute("Sp_PsyProfileVM", parameters, commandType: CommandType.StoredProcedure);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(p);
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
                    parameters.Add("@flag", 3); // Delete
                    parameters.Add("@PsychiatristId", id);
                    connection.Execute("Sp_PsyProfileVM", parameters, commandType: CommandType.StoredProcedure);
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