using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
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
            // Retrieve the list of JobPosts
            var jobPosts = await _context.JobPost
                   .Where(r => r.RecruiterId == user.Id)
                   .ToListAsync();

            ViewBag.JobPosts = jobPosts;
            return View(jobPosts);
        }

        public async Task<IActionResult> CreateJobPost()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // Handle the case when the user is not found
                return NotFound();
            }

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
                    Department = viewModel.Department,
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

            return View(viewModel);
        }


    }
}
