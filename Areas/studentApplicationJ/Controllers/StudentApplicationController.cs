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
                        var searchApplication = await _context.StudentApplication.FirstOrDefaultAsync(a => a.StudentId == student.StudentId && a.JobPostId == jobPost.JobPostId);
                        if (searchApplication != null)
                        {
                            Toaster.AddSuccessToastMessage(TempData, "You already applied to this post");
                            return RedirectToAction("SearchAndApply", "Student", new { area = "studentJ" });
                        }
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

        public async Task<IActionResult> PartialRecruiterStudentApplicationReview(string id)
        {
            var studentId = await _context.StudentApplication
                       .Where(y => y.ApplicationId == id)
                       .Select(y => y.StudentId)
                       .FirstOrDefaultAsync();
            var student = await _context.Student.FindAsync(studentId);
            var studentApplication = await _context.StudentApplication.FirstOrDefaultAsync(r => r.StudentId == studentId);
            if (studentApplication == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(studentId);
            if (user == null)
            {
                return NotFound();
            }
            if (student == null)
            {
                return NotFound();
            }
            var jobPostId = await _context.StudentApplication
                       .Where(y => y.ApplicationId == id)
                       .Select(y => y.JobPostId)
                       .FirstOrDefaultAsync();
            var jobpost = await _context.JobPost.FindAsync(jobPostId);

            if (jobpost == null)
            {
                return NotFound();
            }
            var yearsOfStudy = await _context.YearsOfStudy.FirstOrDefaultAsync(y =>
                  y.JobPostId == jobpost.JobPostId &&
                  (y.IsFirstYear || y.IsSecondYear || y.IsThirdYear || y.IsHonours || y.IsGraduates || y.IsMasters || y.IsPhD || y.IsPostdoc));

            var yearsOfStudyOptions = new List<string>();
            //List<StudentApplication.EnumStudentApplicationStatus?> OutComeStatusList = new List<StudentApplication.EnumStudentApplicationStatus?>()
            //                                                                            {
            //                                                                                null, // Option representing no selection
            //                                                                                StudentApplication.EnumStudentApplicationStatus.Interview,
            //                                                                                StudentApplication.EnumStudentApplicationStatus.OnHold,
            //                                                                                StudentApplication.EnumStudentApplicationStatus.Rejected,
            //                                                                                StudentApplication.EnumStudentApplicationStatus.Appointed,
            //                                                                            };



            if (yearsOfStudy != null)
            {
                if (yearsOfStudy.IsFirstYear)
                    yearsOfStudyOptions.Add("1st Year");
                if (yearsOfStudy.IsSecondYear)
                    yearsOfStudyOptions.Add("2nd Year");
                if (yearsOfStudy.IsThirdYear)
                    yearsOfStudyOptions.Add("3rd Year");
                if (yearsOfStudy.IsHonours)
                    yearsOfStudyOptions.Add("Honours");
                if (yearsOfStudy.IsGraduates)
                    yearsOfStudyOptions.Add("Graduates");
                if (yearsOfStudy.IsMasters)
                    yearsOfStudyOptions.Add("Masters");
                if (yearsOfStudy.IsPhD)
                    yearsOfStudyOptions.Add("PhD");
                if (yearsOfStudy.IsPostdoc)
                    yearsOfStudyOptions.Add("Postdoc");
            }
            var viewmodel = new PartialStudentApplicationViewModel
            {
                StudentApplicationId = id,
                JobTitle = jobpost.JobTitle,
                JobDescription = jobpost.JobDescription,
                Department = jobpost.Faculty.ToString(),
                Course = jobpost.Department.ToString(),
                Levels = yearsOfStudyOptions,

                FirstName = user.FirstName,
                Surname = user.Surname,
                Address = student.Address,
                CareerObjective = student.CareerObjective,
                Skills = student.Skills,
                Achievements = student.Achievements,
                Interests = student.Interests,
                IDNumber = student.IDNumber,
                Race = student.Race.ToString(),
                Gender = student.Gender.ToString(),
                DriversLicense = student.DriversLicense.ToString(),
                Nationality = student.Nationality.ToString(),
                CurrentYearOfStudy = student.CurrentYearOfStudy.ToString(),
                PhoneNumber = user.PhoneNumber,
                Telephone = user.Telephone,
                Email = user.Email,
                SelectedStudentApplicationStatus = studentApplication.StudentApplicationStatus
            };

            return View(viewmodel);
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

        public async Task<IActionResult> SaveStudentApplicationStatus(string id, PartialStudentApplicationViewModel viewmodel)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studenApplication = await _context.StudentApplication.FirstOrDefaultAsync(r => r.ApplicationId == id);

            if (studenApplication == null)
            {
                return NotFound();
            }

            //studenApplication.StudentApplicationStatus = viewmodel.OutComeStatus;
            

            //// Update the Approved property based on the Outcome
            //if (viewModel.Outcome.ToString() == "Approved")
            //{
            //    recruiter.Approved = true;
            //}
            //else
            //{
            //    recruiter.Approved = false;
            //}

            //// Save the changes to the database
            //await _context.SaveChangesAsync();

            return RedirectToAction("ManageRecruiterRegistration");
        }
    }
}
