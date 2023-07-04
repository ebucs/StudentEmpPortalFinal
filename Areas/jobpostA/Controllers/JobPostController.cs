using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace StudentEmploymentPortal.Areas.jobpostA.Controllers
{
    [Area("jobpostA")]
    [Authorize]
    public class JobPostController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public JobPostController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> ManageJobPosts()
        {
            var user = await _userManager.GetUserAsync(User);
            var recruiter = await _context.Recruiter.FindAsync(user.Id);
            // Retrieve the list of JobPosts
            var jobPosts = await _context.JobPost
                .Where(r => r.RecruiterId == user.Id)
                .ToListAsync();

            // Update the department value if null
            foreach (var jobPost in jobPosts)
            {
                if (string.IsNullOrEmpty(jobPost.Department))
                {
                    jobPost.Department = recruiter.TradingName;
                }
            }
            // Set the trading name in ViewBag
            //ViewBag.TradingName = recruiter.TradingName;

            ViewBag.JobPosts = jobPosts;
            return View(jobPosts);
        }


        public async Task<IActionResult> CreateJobPost()
        {
            var user = await _userManager.GetUserAsync(User);
            var recruiter = await _context.Recruiter.FindAsync(user.Id);

            // Set the ViewBag.TradingName
            ViewBag.TradingName = recruiter.TradingName;

            var viewModel = new JobPostViewModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                ClosingDate = DateTime.Now,
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJobPost(JobPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var recruiter = await _context.Recruiter.FindAsync(user.Id);

                if (user == null)
                {
                    // Handle the case when the user is not found
                    return NotFound();
                }

                var jobPost = new JobPost
                {
                    RecruiterId = user.Id,
                    RecruiterType = viewModel.RecruiterType,
                    Faculty = viewModel.Faculty,
                    Department =  viewModel.Department,

                    JobTitle = viewModel.JobTitle,
                    Location = viewModel.Location,
                    JobDescription = viewModel.JobDescription,
                    KeyResponsibilities = viewModel.KeyResponsibilities,
                    JobType = viewModel.JobType,
                    PartTimeNumberOfHours = viewModel.PartTimeNumberOfHours,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    HourlyRate = viewModel.HourlyRate,
                    Nationality = viewModel.Nationality,
                    MinRequirements = viewModel.MinRequirements,
                    ApplicationInstruction = viewModel.ApplicationInstruction,
                    ClosingDate = viewModel.ClosingDate,
                    ContactPerson = viewModel.ContactPerson,
                    Email = viewModel.Email,
                    ContactNo = viewModel.ContactNo,
                    ApprovalStatus = viewModel.ApprovalStatus,
                    Approved = viewModel.Approved,
                    ApproversNote = viewModel.ApproversNote
                };

                var yearsOfStudy = new YearsOfStudy
                {
                    IsFirstYear = viewModel.IsFirstYear,
                    IsSecondYear = viewModel.IsSecondYear,
                    IsThirdYear = viewModel.IsThirdYear,
                    IsHonours = viewModel.IsHonours,
                    IsGraduates = viewModel.IsGraduates,
                    IsMasters = viewModel.IsMasters,
                    IsPhD = viewModel.IsPhD,
                    IsPostdoc = viewModel.IsPostdoc,
                };

                _context.JobPost.Add(jobPost);
                await _context.SaveChangesAsync();

                // Update the JobPostId of the associated YearsOfStudy entity
                yearsOfStudy.JobPostId = jobPost.JobPostId;

                _context.YearsOfStudy.Add(yearsOfStudy);
                await _context.SaveChangesAsync();

                Toaster.AddSuccessToastMessage(TempData, "Job Post Created Successfully.");
                return RedirectToAction("RecruiterDashboard", "Recruiter", new { area = "RecruiterJ" });
            }
            // Log or handle the validation errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Validation Error: " + error.ErrorMessage);
            }
            return View(viewModel);
        }

        public IActionResult EditJobPost(string jobPostId)
        {
            var jobPost = _context.JobPost.FirstOrDefault(q => q.JobPostId == jobPostId);
            var yearsOfStudy = _context.YearsOfStudy.FirstOrDefault(q => q.JobPostId == jobPostId);

            if (jobPost == null)
            {
                // Handle the case when the job post is not found
                return NotFound();
            }

            var viewModel = new editjobpostViewModel
            {
                // Populate the view model properties with the existing job post data
                JobpostId = jobPost.JobPostId,
                RecruiterType = jobPost.RecruiterType,
                Faculty = jobPost.Faculty,
                Department = jobPost.Department,
                JobTitle = jobPost.JobTitle,
                Location = jobPost.Location,
                JobDescription = jobPost.JobDescription,
                KeyResponsibilities = jobPost.KeyResponsibilities,
                JobType = jobPost.JobType,
                PartTimeNumberOfHours = (JobPost.EnumWeekHours)jobPost.PartTimeNumberOfHours,
                StartDate = jobPost.StartDate,
                EndDate = jobPost.EndDate,
                HourlyRate = jobPost.HourlyRate,
                Nationality = jobPost.Nationality,
                MinRequirements = jobPost.MinRequirements,
                ApplicationInstruction = jobPost.ApplicationInstruction,
                ClosingDate = jobPost.ClosingDate,
                ContactPerson = jobPost.ContactPerson,
                Email = jobPost.Email,
                ContactNo = jobPost.ContactNo,
                ApprovalStatus = jobPost.ApprovalStatus,
                Approved = jobPost.Approved,
                ApproversNote = jobPost.ApproversNote,
                IsFirstYear = yearsOfStudy?.IsFirstYear ?? false,
                IsSecondYear = yearsOfStudy?.IsSecondYear ?? false,
                IsThirdYear = yearsOfStudy?.IsThirdYear ?? false,
                IsHonours = yearsOfStudy?.IsHonours ?? false,
                IsGraduates = yearsOfStudy?.IsGraduates ?? false,
                IsMasters = yearsOfStudy?.IsMasters ?? false,
                IsPhD = yearsOfStudy?.IsPhD ?? false,
                IsPostdoc = yearsOfStudy?.IsPostdoc ?? false
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditJobPost(editjobpostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var jobPost = await _context.JobPost.FindAsync(viewModel.JobpostId);

                if (jobPost == null)
                {
                    // Handle the case when the job post is not found
                    return NotFound();
                }

                jobPost.RecruiterType = viewModel.RecruiterType;
                jobPost.Faculty = viewModel.Faculty;
                jobPost.Department = viewModel.Department;
                jobPost.JobTitle = viewModel.JobTitle;
                jobPost.Location = viewModel.Location;
                jobPost.JobDescription = viewModel.JobDescription;
                jobPost.KeyResponsibilities = viewModel.KeyResponsibilities;
                jobPost.JobType = viewModel.JobType;
                jobPost.PartTimeNumberOfHours = viewModel.PartTimeNumberOfHours;
                jobPost.StartDate = viewModel.StartDate;
                jobPost.EndDate = viewModel.EndDate;
                jobPost.HourlyRate = viewModel.HourlyRate;
                jobPost.Nationality = viewModel.Nationality;
                jobPost.MinRequirements = viewModel.MinRequirements;
                jobPost.ApplicationInstruction = viewModel.ApplicationInstruction;
                jobPost.ClosingDate = viewModel.ClosingDate;
                jobPost.ContactPerson = viewModel.ContactPerson;
                jobPost.Email = viewModel.Email;
                jobPost.ContactNo = viewModel.ContactNo;
                jobPost.ApprovalStatus = viewModel.ApprovalStatus;
                jobPost.Approved = viewModel.Approved;
                jobPost.ApproversNote = viewModel.ApproversNote;

                var yearsOfStudy = await _context.YearsOfStudy.FirstOrDefaultAsync(q => q.JobPostId == viewModel.JobpostId);

                if (yearsOfStudy == null)
                {
                    // If the job post doesn't have an associated YearsOfStudy entity, create a new one
                    yearsOfStudy = new YearsOfStudy();
                    yearsOfStudy.JobPostId = viewModel.JobpostId;
                    _context.YearsOfStudy.Add(yearsOfStudy);
                }

                yearsOfStudy.IsFirstYear = viewModel.IsFirstYear;
                yearsOfStudy.IsSecondYear = viewModel.IsSecondYear;
                yearsOfStudy.IsThirdYear = viewModel.IsThirdYear;
                yearsOfStudy.IsHonours = viewModel.IsHonours;
                yearsOfStudy.IsGraduates = viewModel.IsGraduates;
                yearsOfStudy.IsMasters = viewModel.IsMasters;
                yearsOfStudy.IsPhD = viewModel.IsPhD;
                yearsOfStudy.IsPostdoc = viewModel.IsPostdoc;

                await _context.SaveChangesAsync();

                Toaster.AddSuccessToastMessage(TempData, "Job Post Updated Successfully.");
                return RedirectToAction("ManageJobPosts", "JobPost", new { area = "jobpostA", jobPostId = jobPost.JobPostId });
            }

            // Log or handle the validation errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine("Validation Error: " + error.ErrorMessage);
            }
            return View(viewModel);
        }



        [HttpGet]
        public List<string> GetDepartments(string faculty)
        {
            
            
            List<string> CommerceLawAndManagement = new List<string>() { "Economic And Business Sciences",
                                                                         "Finance And Investment Management",
                                                                         "Industrial Psychology And People Management",
                                                                         "Chemical Engineering",
                                                                         "Law"};



            List<string> EngineeringAndBuiltEnvironment = new List<string>() { "Chemical Engineering",
                                                                               "Civil And Environmental Engineering",
                                                                               "Electrical And Information Engineering",
                                                                               "Mechanical Industrial And Aeronautical Engineering"};
            List<string> HealthSciences = new List<string>() { "Anatomy",
                                                               "Dentistry",
                                                               "Medicine",
                                                               "Pharmacy And Pharmacology"};
            List<string> Humanities = new List<string>() { "Archaeology And Anthropology",
                                                           "Geography Archaeology And Environmental Studies",
                                                           "Political Studies And International Relations"};
            List<string> Science = new List<string>() { "Chemistry",
                                                        "Mathematics",
                                                        "Physics",
                                                        "Zoology And Entomology",
                                                        "Computer Science",
                                                        "Geosciences",
                                                        "Human Physiology",
                                                        "Molecular Medicine And Haematology",
                                                        "School Of Accountancy"};



            if (faculty.Equals("CommerceLawAndManagement"))
            {
                return CommerceLawAndManagement;
            }
            else if (faculty.Equals("EngineeringAndBuiltEnvironment"))



            {
                return EngineeringAndBuiltEnvironment;
            }
            else if (faculty.Equals("HealthSciences"))



            {
                return HealthSciences;
            }
            else if (faculty.Equals("Humanities"))



            {
                return Humanities;
            }
            else 
            {
                return Science;
            }

          
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawJobPost(string jobPostId)
        {
            var jobPost = await _context.JobPost.FindAsync(jobPostId);

            if (jobPost == null)
            {
                return NotFound();
            }

            jobPost.ApprovalStatus = JobPost.EnumApprovalStatus.Withdrawn;
            await _context.SaveChangesAsync();
            Toaster.AddSuccessToastMessage(TempData, "Job Post withdrawn Successfully.");

            return RedirectToAction("ManageJobPosts", "JobPost", new { area = "jobpostA" });
        }
    }
}
