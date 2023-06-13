using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Models;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;

namespace StudentEmploymentPortal.Controllers
{
    public class JobPostController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; // Add the DbContext

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
            if (user != null)
            {
                // Retrieve the RecruiterId from the ApplicationUser
                string RecruiterId = user.Id;

                // Pass the FirstName to the view
                ViewData["RecruiterId"] = RecruiterId;

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveJobPost(JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                // Add the jobPost to the context
                jobPost.ApprovalStatus = JobPost.EnumApprovalStatus.Pending;
                jobPost.StartDate = jobPost.StartDate.Date;
                
                _context.JobPost.Add(jobPost);

                // Save the changes
                await _context.SaveChangesAsync();
                Toaster.AddSuccessToastMessage(TempData, "Job post added successfully.");
                return RedirectToAction("ManageJobPosts");
            }
            // If the model is not valid, return the same view with the validation errors
            return View("CreateJobPost", jobPost);

        }
    }
}
