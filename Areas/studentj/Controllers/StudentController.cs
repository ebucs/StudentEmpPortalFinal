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
using StudentEmploymentPortal.Migrations;

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
                var student = await _context.Student.Include(s => s.Qualifications)
                    .FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (student != null)
                {
                    var viewModel = new BuildProfile
                    {
                        StudentProfile = new ManageStudentProfileViewModel
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
                        },
                        Qualifications = student.Qualifications.Select(q => new QualificationViewModel
                        {
                            QualificationId = q.QualificationId,
                            Institution = q.Institution,
                            Date = q.Date,
                            Qualification = q.StuQualification,
                            Subjects = q.Subjects,
                            Majors = q.Majors,
                            Submajors = q.Submajors,
                            Research = q.Research
                        }).ToList()
                    };
                    // Return the full view with the viewModel
                    return View(viewModel);

                }
            }

            // Handle the case when the user is not found
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProfile(BuildProfile viewModel)
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
                        student.CareerObjective = viewModel.StudentProfile.CareerObjective;
                        student.Skills = viewModel.StudentProfile.Skills;
                        student.Achievements = viewModel.StudentProfile.Achievements;
                        student.Interests = viewModel.StudentProfile.Interests;
                        user.PhoneNumber = viewModel.StudentProfile.PhoneNumber;
                        user.Telephone = viewModel.StudentProfile.Telephone;
                        student.IDNumber = viewModel.StudentProfile.IDNumber;
                        student.Race = viewModel.StudentProfile.SelectedRace;
                        student.Gender = viewModel.StudentProfile.SelectedGender;
                        student.Address = viewModel.StudentProfile.Address;
                        student.DriversLicense = viewModel.StudentProfile.SelectedDriversLicense;
                        student.Nationality = viewModel.StudentProfile.SelectedNationality;
                        student.CurrentYearOfStudy = viewModel.StudentProfile.SelectedCurrentYearOfStudy;
                        student.Faculty = viewModel.StudentProfile.SelectedFaculty;
                        student.Department = viewModel.StudentProfile.SelectedDepartment;

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
                var studentYearOfStudy = student.CurrentYearOfStudy.ToString();

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

        //Qualifications

        public IActionResult AddQualificationpv()
        {
            var viewModel = new QualificationViewModel
            {
                QualificationId = Guid.NewGuid().ToString(), // Generate a new qualification ID,
            };

            return PartialView(viewModel);
        }



        // HTTP POST method to save the addition of a qualification
        [HttpPost]
        public async Task<IActionResult> AddQualification(QualificationViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {

                    // Create a new Qualification entity and map the properties from the view model
                    var newQualification = new Qualification
                    {
                        QualificationId = viewModel.QualificationId,
                        Institution = viewModel.Institution,
                        Date = viewModel.Date,
                        StuQualification = viewModel.Qualification,
                        Subjects = viewModel.Subjects,
                        Majors = viewModel.Majors,
                        Submajors = viewModel.Submajors,
                        Research = viewModel.Research,
                        StudentId = student.StudentId // Set the StudentId using the current student's ID
                    };

                    _context.Qualification.Add(newQualification);
                    await _context.SaveChangesAsync();
                    Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    return RedirectToAction("ManageProfile");
                }
            }

            // Handle invalid model state, user, or qualification
            return RedirectToAction("ManageProfile");
        }

        public IActionResult EditQualification(string qualificationId)
        {
            var qualification = _context.Qualification.FirstOrDefault(q => q.QualificationId == qualificationId);

            if (qualification != null)
            {
                var viewModel = new QualificationViewModel
                {
                    QualificationId = qualification.QualificationId,
                    Institution = qualification.Institution,
                    Date = qualification.Date,
                    Qualification = qualification.StuQualification,
                    Subjects = qualification.Subjects,
                    Majors = qualification.Majors,
                    Submajors = qualification.Submajors,
                    Research = qualification.Research
                };

                return PartialView(viewModel);
            }

            // Handle qualification not found
            return RedirectToAction("ManageProfile");
        }


        [HttpPost]
        public async Task<IActionResult> EditQualification(QualificationViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {
                    var qualification = await _context.Qualification.FindAsync(viewModel.QualificationId);

                    if (qualification != null)
                    {
                        qualification.Institution = viewModel.Institution;
                        qualification.Date = viewModel.Date;
                        qualification.StuQualification = viewModel.Qualification;
                        qualification.Subjects = viewModel.Subjects;
                        qualification.Majors = viewModel.Majors;
                        qualification.Submajors = viewModel.Submajors;
                        qualification.Research = viewModel.Research;

                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                        return RedirectToAction("ManageProfile");
                    }
                }
            }

            // Handle invalid model state, user, or qualification
            return RedirectToAction("ManageProfile");
        }



        // HTTP POST method to delete a qualification
        [HttpPost]
        public async Task<IActionResult> DeleteQualification(QualificationViewModel viewModel)
        {
            var qualification = await _context.Qualification.FindAsync(viewModel.QualificationId);

            if (qualification != null)
            {
                _context.Qualification.Remove(qualification);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageProfile");
        }









        public IActionResult ApplicationsHistory()
        {
            return View();
        }
    }
}
