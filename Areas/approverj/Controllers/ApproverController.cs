using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;

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
                    .Where(r => r.Approved == false)
                    .ToListAsync();

                // Pass the recruiters and UserManager to the view
                ViewBag.Recruiters = recruiters;
                ViewBag.UserManager = _userManager;

                return View(recruiters);
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



    }

}
