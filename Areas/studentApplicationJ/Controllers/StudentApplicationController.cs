using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using StudentEmploymentPortal.ViewModels.StudentViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using StudentEmploymentPortal.ViewModels.StudentApplicationViewModels;
using static StudentEmploymentPortal.Areas.studentApplicationJ.Models.StudentApplication;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Controllers
{
    [Area("StudentApplicationJ")]
    public class StudentApplicationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentApplicationController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateApplication(string JobPostId)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var student = await _context.Student.FindAsync(user.Id);

                if (student != null)
                {
                    // Retrieve the jobpost based on the id
                    var jobPost = await _context.JobPost.FirstOrDefaultAsync(r => r.JobPostId == JobPostId);

                    if (jobPost != null)
                    {
                        //var searchApplication = await _context.StudentApplication.FirstOrDefaultAsync(a => a.StudentId == student.StudentId && a.JobPostId == jobPost.JobPostId);
                        //if (searchApplication != null)
                        //{
                        //    //Toaster.AddSuccessToastMessage(TempData, "You already applied to this post");
                        //    return RedirectToAction("SearchAndApply", "Student", new { area = "studentJ" });
                        //}
                        var application = new StudentApplication
                        {
                            StudentId = student.StudentId,
                            JobPostId = JobPostId,
                            RecruiterId = jobPost.RecruiterId,
                            DateCreated = DateTime.Now,
                            StudentApplicationStatus = StudentApplication.EnumStudentApplicationStatus.Pending
                        };

                        _context.StudentApplication.Add(application);

                        int saveApplication = await _context.SaveChangesAsync();

                        if (saveApplication > 0)
                        {
                            var StudentApplication = await _context.StudentApplication.FirstOrDefaultAsync(a => a.StudentId == student.StudentId && a.JobPostId == jobPost.JobPostId);

                            if (StudentApplication != null)
                            {
                                return RedirectToAction("AddApplicationDocument", "StudentApplication", new { area = "StudentApplicationJ", id = StudentApplication.ApplicationId });
                            }
                        }
                    }
                }
            }


            return RedirectToAction("SearchAndApply", "Student", new { area = "studentJ" });
        }


        public IActionResult AddApplicationDocument(string id)
        {
            ViewData["ApplicationId"] = id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddApplicationDocument(IFormFile file, string DocumentName, string ApplicationId)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var application = await _context.StudentApplication.FindAsync(ApplicationId);

                if (application != null)
                {
                    var applicationDocument = new ApplicationDocument
                    {
                        ApplicationId = ApplicationId,
                        DocumentName = DocumentName,
                        FilePath = uniqueFileName
                    };

                    _context.ApplicationDocument.Add(applicationDocument);
                    await _context.SaveChangesAsync();

                    Toaster.AddSuccessToastMessage(TempData, "Application submitted.");

                    return RedirectToAction("SearchAndApply", "Student", new { area = "StudentJ" });
                }
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> PartialStudentApplicationDetails(string id)
        {
            var jobPostId = await _context.StudentApplication
                       .Where(y => y.ApplicationId == id)
                       .Select(y => y.JobPostId)
                       .FirstOrDefaultAsync();
            var jobPost = await _context.JobPost.FirstOrDefaultAsync(r => r.JobPostId == jobPostId);

            var viewModel = new StudentJobPostViewModel
            {
                JobPostId = jobPost.JobPostId,
                JobTitle = jobPost.JobTitle,
                Location = jobPost.Location,
                JobDescription = jobPost.JobDescription,
                KeyResponsibilities = jobPost.KeyResponsibilities,
                JobType = jobPost.JobType,
                PartTimeNumberOfHours = (JobPost.EnumWeekHours)jobPost.PartTimeNumberOfHours,
                StartDate = jobPost.StartDate,
                EndDate = jobPost.EndDate,
                HourlyRate = jobPost.HourlyRate,
                MinRequirements = jobPost.MinRequirements,
                ApplicationInstruction = jobPost.ApplicationInstruction,
                ClosingDate = jobPost.ClosingDate,


            };

            return PartialView(viewModel);

        }
    }
}