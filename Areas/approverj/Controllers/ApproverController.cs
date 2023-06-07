using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Data;

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
            var user = _userManager.GetUserAsync(User);

        }
    }

}
