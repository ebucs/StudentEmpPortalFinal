using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using StudentEmploymentPortal.ViewModels.StudentViewModels;

namespace StudentEmploymentPortal.Areas.studentj.Controllers
{
    [Area("StudentJ")]
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context; // Add the DbContext


        public StudentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> StudentDashboard()
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

            return View();
        }

        public async Task<IActionResult> ManageProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (student != null)
                {
                    var viewModel = new ManageStudentProfileViewModel
                    {
                        // Editable fields
                        CareerObjective = student.CareerObjective,
                        Skills = student.Skills,
                        Achievements = student.Achievements,
                        Interests = student.Interests,
                        PhoneNumber = user.PhoneNumber,
                        Telephone = user.Telephone,
                        IDNumber = student.IDNumber,
                        Address = student.Address,
                        SelectedRace = student.Race,
                        SelectedGender = student.Gender,
                        SelectedDriversLicense = student.DriversLicense,
                        SelectedNationality = student.Nationality,
                        SelectedCurrentYearOfStudy = student.CurrentYearOfStudy,
                        SelectedFaculty = student.Faculty,
                        SelectedDepartment = student.Department,

                        // Non-editable fields
                        FirstName = user.FirstName,
                        Surname = user.Surname,
                        Email = user.Email,
                    };

                    return View(viewModel);
                }
                else
                {
                    // Handle the case when the student is not found
                    var viewModel = new ManageStudentProfileViewModel
                    {
                        // Editable fields
                        CareerObjective = string.Empty,
                        Skills = string.Empty,
                        Achievements = string.Empty,
                        Interests = string.Empty,
                        PhoneNumber = user.PhoneNumber,
                        Telephone = user.Telephone,
                        IDNumber = string.Empty,
                        Address = string.Empty,
                        SelectedRace = default,
                        SelectedGender = default,
                        SelectedDriversLicense = default,
                        SelectedNationality = default,
                        SelectedCurrentYearOfStudy = default,
                        SelectedFaculty = default,
                        SelectedDepartment = default,

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
        public async Task<IActionResult> SaveProfile(ManageStudentProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var student = await _context.Student.FindAsync(user.Id);

                    if (student != null)
                    {
                        // Update the student profile
                        student.CareerObjective = viewModel.CareerObjective;
                        student.Skills = viewModel.Skills;
                        student.Achievements = viewModel.Achievements;
                        student.Interests = viewModel.Interests;
                        user.PhoneNumber = viewModel.PhoneNumber;
                        user.Telephone = viewModel.Telephone;
                        student.IDNumber = viewModel.IDNumber;
                        student.Race = viewModel.SelectedRace;
                        student.Gender = viewModel.SelectedGender;
                        student.Address = viewModel.Address;
                        student.DriversLicense = viewModel.SelectedDriversLicense;
                        student.Nationality = viewModel.SelectedNationality;
                        student.CurrentYearOfStudy = viewModel.SelectedCurrentYearOfStudy;
                        student.Faculty = viewModel.SelectedFaculty;
                        student.Department = viewModel.SelectedDepartment;

                        // Save the changes
                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    }
                    else
                    {
                        // Create a new student instance and populate its properties
                        student = new Student
                        {
                            StudentId = user.Id,
                            CareerObjective = viewModel.CareerObjective,
                            Skills = viewModel.Skills,
                            Achievements = viewModel.Achievements,
                            Interests = viewModel.Interests,
                            IDNumber = viewModel.IDNumber,
                            Race = viewModel.SelectedRace,
                            Address = viewModel.Address,
                            Gender = viewModel.SelectedGender,
                            DriversLicense = viewModel.SelectedDriversLicense,
                            Nationality = viewModel.SelectedNationality,
                            CurrentYearOfStudy = viewModel.SelectedCurrentYearOfStudy,
                            Faculty = viewModel.SelectedFaculty,
                            Department = viewModel.SelectedDepartment
                        };

                        // Add the student to the context
                        _context.Student.Add(student);

                        // Save the changes
                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    }
                }
                return RedirectToAction("ManageProfile");
            }

            // If the model is not valid, return the same view with the validation errors
            return View("ManageProfile", viewModel);
        }

        public async Task<IActionResult> SearchAndApply()
        {
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Student.FindAsync(user.Id);
            var jobPostsIds = new List<string>();
            var jobPosts = new List<JobPost>();

            if (student != null)
            {
                var studentYearOfStudy = student.CurrentYearOfStudy;

                if (studentYearOfStudy.Equals("FirstYear"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsFirstYear)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("SecondYear"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsSecondYear)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("ThirdYear"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsThirdYear)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("Honours"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsHonours)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("Graduates"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsGraduates)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("Masters"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsMasters)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else if (studentYearOfStudy.Equals("PhD"))
                {
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsPhD)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                else
                { 
                    jobPostsIds = await _context.YearsOfStudy
                        .Where(y => y.IsPostdoc)
                        .Select(y => y.JobPostId)
                        .ToListAsync();
                }
                

                foreach (var jobPostId in jobPostsIds)
                {
                    var jobPost = await _context.JobPost.FindAsync(jobPostId);
                    jobPosts.Add(jobPost);
                }

            }
            else
            {
                ViewData["Message"] = "Complete profile to view posts";
            }

            return View(jobPosts);
        }

        public async Task<IActionResult> StudentPartialJobPostDetails(string id)
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

           
            // Create a new instance of JobPostViewModel and populate it with the data
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


        public IActionResult ApplicationsHistory()
        {
            return View();
        }
    }
}
