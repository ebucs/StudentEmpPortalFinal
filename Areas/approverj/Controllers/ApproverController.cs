using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.ViewModels;

namespace StudentEmploymentPortal.Areas.approverj.Controllers
{
    [Area("ApproverJ")]
    [Authorize]
    public class ApproverController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; // Add the DbContext


        public ApproverController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> ApproverDashboard()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Retrieve the FirstName from the ApplicationUser
                string firstName = user.FirstName;

                // Pass the FirstName to the view
                ViewData["FirstName"] = firstName;
            }

            return View("ApproverDashboard");
        }

        public async Task<IActionResult> ManageRecruiterRegistration()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Retrieve the list of recruiters with Approved = false
                var recruiters = await _context.Recruiter
                    .Where(r => r.Approved == false || r.Approved == true)
                    .ToListAsync();

                // Pass the recruiters and UserManager to the view
                ViewBag.Recruiters = recruiters;
                ViewBag.UserManager = _userManager;

                return View(recruiters);
            }

            return NotFound();
        }

        public async Task<IActionResult> ManageRecruiterJobPosts()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Retrieve the list of recruiters with Approved = false
                var jobPosts = await _context.JobPost
                    .Where(r => r.Approved == false || r.Approved == true)
                    .ToListAsync();

                // Pass the job posts to the view
                return View(jobPosts);
            }

            return NotFound();
        }


        public async Task<IActionResult> PartialRecruiterDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the recruiter based on the id
            var recruiter = await _context.Recruiter.FirstOrDefaultAsync(r => r.RecruiterId == id);

            if (recruiter == null)
            {
                return NotFound();
            }

            // Retrieve the user associated with the recruiter
            var user = await _userManager.FindByIdAsync(recruiter.RecruiterId);

            // Create a new instance of CompleteRegisterViewModel and populate it with the data
            var viewModel = new CompleteRegisterViewModel
            {
                Title = recruiter.Title,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Telephone = user.Telephone,
                RegisteredAddress = recruiter.RegisteredAddress,
                JobTitle = recruiter.JobTitle,
                RegistrationNumber = recruiter.RegistrationNumber,
                RegisteredName = recruiter.RegisteredName,
                TradingName = recruiter.TradingName,
                BusinessType = recruiter.BusinessType,
                // Populate additional properties specific to CompleteRegisterViewModel
                // For example:
                ApproverNote = recruiter.ApproversNote,
                Outcome = recruiter.OutcomeStatus
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialRecruiterDetails(string id, CompleteRegisterViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the recruiter based on the id
            var recruiter = await _context.Recruiter.FirstOrDefaultAsync(r => r.RecruiterId == id);

            if (recruiter == null)
            {
                return NotFound();
            }

            // Update the recruiter's note and outcome based on the submitted data
            recruiter.ApproversNote = viewModel.ApproverNote;
            recruiter.OutcomeStatus = viewModel.Outcome;

            // Update the Approved property based on the Outcome
            if (viewModel.Outcome.ToString() == "Approved")
            {
                recruiter.Approved = true;
            }
            else
            {
                recruiter.Approved = false;
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageRecruiterRegistration");
        }


        public async Task<IActionResult> PartialJobPostDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the jobpost based on the id
            var jobPost = await _context.JobPost.FirstOrDefaultAsync(r => r.JobPostId == id);

            if (jobPost == null)
            {
                return NotFound();
            }

            var yearsOfStudy = await _context.YearsOfStudy.FirstOrDefaultAsync(y =>
                y.JobPostId == jobPost.JobPostId &&
                (y.IsFirstYear || y.IsSecondYear || y.IsThirdYear || y.IsHonours || y.IsGraduates || y.IsMasters || y.IsPhD || y.IsPostdoc)
            );

            var yearsOfStudyOptions = new List<string>();
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

            // Create a new instance of JobPostViewModel and populate it with the data
            var viewModel = new JobPostViewModel
            {
                RecruiterType = jobPost.RecruiterType,
                Faculty = (JobPost.EnumFaculty)jobPost.Faculty,
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

                YearsOfStudyOptions = yearsOfStudyOptions
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialJobPostDetails(string id, JobPostViewModel viewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the jobpost based on the id
            var jobPost = await _context.JobPost.FirstOrDefaultAsync(r => r.JobPostId == id);

            if (jobPost == null)
            {
                return NotFound();
            }

            // Update the recruiter's note and outcome based on the submitted data
            jobPost.ApproversNote = viewModel.ApproversNote;
            jobPost.ApprovalStatus = viewModel.ApprovalStatus;

            // Update the Approved property based on the Outcome
            if (viewModel.ApprovalStatus.ToString() == "Approved")
            {
                jobPost.Approved = true;
            }
            else
            {
                jobPost.Approved = false;
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageRecruiterJobPosts");
        }

      
        public IActionResult RecruiterTypeJobPostsGraph()
        {
            // Retrieve the earliest and latest dates from the table
            var earliestDate = _context.JobPost.Min(j => j.StartDate);
            var latestDate = _context.JobPost.Max(j => j.EndDate);

            // Calculate the start date by subtracting 12 months from the latest date
            var startDate = latestDate.AddMonths(-12);

            // Retrieve the counts of internal and external job posts within the last 12 months
            var jobPostCounts = _context.JobPost
                .Where(j => j.StartDate >= startDate && j.EndDate <= latestDate)
                .GroupBy(j => j.RecruiterType)
                .Select(g => new { RecruiterType = g.Key, Count = g.Count() })
                .ToList();

            // Prepare the data for the recruiter type chart
            var recruiterTypeLabels = new List<string>();
            var recruiterTypeData = new List<int>();

            // If the query returned both internal and external counts, populate the data accordingly
            foreach (var jobPostCount in jobPostCounts)
            {
                if (jobPostCount.RecruiterType == JobPost.EnumRecruiterType.Internal)
                    recruiterTypeLabels.Add("Internal");
                else if (jobPostCount.RecruiterType == JobPost.EnumRecruiterType.External)
                    recruiterTypeLabels.Add("External");

                recruiterTypeData.Add(jobPostCount.Count);
            }

            // Retrieve the distinct departments from the table
            var departments = _context.JobPost.Select(j => j.Department).Distinct().ToList();

            // Prepare the data for the hourly rates by department chart
            var hourlyRatesLabels = departments;
            var hourlyRatesData = new List<int>();

            foreach (var department in departments)
            {
                // Calculate the average hourly rate for each department
                var averageHourlyRate = _context.JobPost
                    .Where(j => j.Department == department)
                    .Average(j => j.HourlyRate);

                hourlyRatesData.Add((int)averageHourlyRate);
            }

            // Create a new instance of the combined view model and populate it with the data
            var viewModel = new BarChartViewModel
            {
                RecruiterTypeLabels = recruiterTypeLabels,
                RecruiterTypeData = recruiterTypeData,
                HourlyRatesLabels = hourlyRatesLabels,
                HourlyRatesData = hourlyRatesData,
                StartDate = startDate,
                EndDate = latestDate
            };

            return View(viewModel);
        }


    }

}
