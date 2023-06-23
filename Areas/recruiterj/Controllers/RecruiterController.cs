﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;

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

        //complete registration method
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
                        ApproverNote = string.Empty,
                        Outcome = recruiter.OutcomeStatus
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
                        ApproverNote = string.Empty,
                        Outcome = default
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
                            Approved = viewModel.Approved,
                            ApproversNote = viewModel.ApproverNote,
                            OutcomeStatus = viewModel.Outcome
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

        // GET: Display the recruiter profile for editing
        public async Task<IActionResult> ManageProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var recruiter = await _context.Recruiter.FindAsync(user.Id);

                if (recruiter != null)
                {
                    var viewModel = new ManageRecruiterProfileViewModelcs
                    {
                        // Editable fields
                        Title = recruiter.Title,
                        JobTitle = recruiter.JobTitle,
                        RegistrationNumber = recruiter.RegistrationNumber,
                        RegisteredName = recruiter.RegisteredName,
                        TradingName = recruiter.TradingName,
                        BusinessType = recruiter.BusinessType,
                        RegisteredAddress = recruiter.RegisteredAddress,
                        PhoneNumber = user.PhoneNumber,
                        Telephone = user.Telephone,

                        // Non-editable fields
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email,
                        ApproverNote = recruiter.ApproversNote,
                        Outcome = recruiter.OutcomeStatus
                    };

                    return View(viewModel);
                }
            }

            // Redirect to the RecruiterDashboard action method if the recruiter is not found
            return RedirectToAction("RecruiterDashboard");
        }

        // POST: Save the changes made to the recruiter profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageProfile(ManageRecruiterProfileViewModelcs viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var recruiter = await _context.Recruiter.FindAsync(user.Id);

                    if (recruiter != null)
                    {
                        // Update the recruiter's profile with the values from the viewModel
                        recruiter.Title = viewModel.Title;
                        recruiter.JobTitle = viewModel.JobTitle;
                        recruiter.RegistrationNumber = viewModel.RegistrationNumber;
                        recruiter.RegisteredName = viewModel.RegisteredName;
                        recruiter.TradingName = viewModel.TradingName;
                        recruiter.BusinessType = viewModel.BusinessType;
                        recruiter.RegisteredAddress = viewModel.RegisteredAddress;
                        user.PhoneNumber = viewModel.PhoneNumber;
                        user.Telephone = viewModel.Telephone;

                        // Save the changes to the context
                        await _context.SaveChangesAsync();

                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    }
                }

                // Redirect to the RecruiterDashboard action method
                return RedirectToAction("RecruiterDashboard");
            }

            // If the model is not valid, return the same view with the validation errors
            return View(viewModel);
        }

        public async Task<IActionResult> ReviewApplications()
        {
            
            var user = await _userManager.GetUserAsync(User);
            var recruiter = await _context.Recruiter.FindAsync(user.Id);


            if (recruiter != null)
            {
                var studentApplications = await _context.StudentApplication
                   .Where(r => r.RecruiterId == recruiter.RecruiterId)
                   .ToListAsync();

                foreach (var students in studentApplications)
                {
                    var studentsWhoApplied = await _context.Student
                   .Where(r => r.StudentId == students.StudentId)
                   .ToListAsync();

                    ViewBag.StudentApplications = studentApplications;
                    return View(studentsWhoApplied);
                }
            }

            return NotFound();
        }


    }
}
