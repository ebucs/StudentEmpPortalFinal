﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.ViewModels.StudentViewModels;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Controllers
{
    [Area("StudentApplicationJ")]
    public class StudentApplicationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public StudentApplicationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
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
                        var application = new StudentApplication
                        {
                            StudentId = student.StudentId,
                            JobPostId = JobPostId,
                            RecruiterId = jobPost.RecruiterId,
                            DateCreated = DateTime.Now
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
    }
}