using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using System.Net;

namespace StudentEmploymentPortal.Areas.recruiterj.Controllers
{
    [Area("RecruiterJ")]
    [Authorize]
    public class RecruiterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; // Add the DbContext


        public RecruiterController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> RecruiterDashboard()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Retrieve the FirstName from the ApplicationUser
                string firstName = user.FirstName;

                // Pass the FirstName to the view
                ViewData["FirstName"] = firstName;

                // Check if the user is approved
                var recruiter = await _context.Recruiter.FindAsync(user.Id);
                bool isApproved = recruiter?.Approved ?? false;
                ViewData["ApprovalStatus"] = isApproved;
            }

            return View("RecruiterDashboard");
        }


        public async Task<IActionResult> CompleteRegister()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var recruiter = await _context.Recruiter.FindAsync(user.Id);

                if (recruiter != null)
                {
                    var viewModel = new CompleteRegisterViewModel
                    {
                        // Editable fields
                      
                        Title = recruiter.Title,
                        JobTitle = recruiter.JobTitle,
                        RegistrationNumber = recruiter.RegistrationNumber,
                        RegisteredName = recruiter.RegisteredName,
                        TradingName = recruiter.TradingName,
                        BusinessType = recruiter.BusinessType,
                        RegisteredAddress = recruiter.RegisteredAddress,
                        ConfirmDetails = recruiter.ConfirmDetails,
                        Approved = recruiter.Approved,
                        PhoneNumber = user.PhoneNumber,
                        Telephone = user.Telephone,

                        // Non-editable fields
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email,
                     };

                    return View(viewModel);
                }
                else
                {
                    // Handle the case when the recruiter is not found
                    var viewModel = new CompleteRegisterViewModel
                    {
                        // Editable fields
                        Title = string.Empty,
                        JobTitle = string.Empty,
                        RegistrationNumber = string.Empty,
                        RegisteredName = string.Empty,
                        TradingName = string.Empty,
                        BusinessType = string.Empty,
                        RegisteredAddress = string.Empty,
                        ConfirmDetails = false,
                        Approved = false,
                        PhoneNumber = user.PhoneNumber,
                        Telephone = user.Telephone,

                        // Non-editable fields
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email,
                    };

                    return View(viewModel);
                }
            }

            // Handle the case when the user is not found
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRecruiter(CompleteRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var recruiter = await _context.Recruiter.FindAsync(user.Id);

                    //if (recruiter != null)
                    //{
                    //    // Update the student profile

                    //    recruiter.Title = viewModel.Title;
                    //    recruiter.JobTitle = viewModel.JobTitle;
                    //    recruiter.RegistrationNumber = viewModel.RegistrationNumber;
                    //    recruiter.RegisteredName = viewModel.RegisteredName;
                    //    recruiter.TradingName = viewModel.TradingName;
                    //    recruiter.BusinessType = viewModel.BusinessType;
                    //    recruiter.RegisteredAddress = viewModel.RegisteredAddress;
                    //    recruiter.ConfirmDetails = viewModel.ConfirmDetails;
                    //    recruiter.Approved = viewModel.Approved;
                    //    user.PhoneNumber = viewModel.PhoneNumber;
                    //    user.Telephone = viewModel.Telephone;

                    //    // Save the changes
                    //    await _context.SaveChangesAsync();
                    //    Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    //}
                    //else
                    if(recruiter == null)
                    {
                        // Create a new student instance and populate its properties
                        recruiter = new Recruiter
                        { 
                            RecruiterId = user.Id,
                            Title = viewModel.Title,
                            JobTitle = viewModel.JobTitle,
                            RegistrationNumber = viewModel.RegistrationNumber,
                            RegisteredName = viewModel.RegisteredName,
                            TradingName = viewModel.TradingName,
                            BusinessType = viewModel.BusinessType,
                            RegisteredAddress = viewModel.RegisteredAddress,
                            ConfirmDetails = viewModel.ConfirmDetails,
                            Approved = viewModel.Approved
                        };

                        // Add the student to the context
                        _context.Recruiter.Add(recruiter);

                        // Save the changes
                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Registration Complete.");
                            
                           

                    }

                }
                return RedirectToAction("RecruiterDashboard");
            }
            // If the model is not valid, return the same view with the validation errors
            return View("CompleteRegister",viewModel);
        }
    }
}
