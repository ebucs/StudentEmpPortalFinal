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
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;

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
                var student = await _context.Student
                    .Include(s => s.Qualifications)
                    .Include(s => s.WorkExperience)
                    .Include(s => s.Referee)
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
                            Department = student.Department,

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
                            Research = q.Research,
                        }).ToList(),
                        Referees = student.Referee.Select(q => new RefereeViewModel
                        {
                            RefereeId = q.RefereeId,
                            FullName = q.FullName,
                            JobTitle = q.JobTitle,
                            Institution = q.Institution,
                            Cellphone = q.Cellphone,
                            Email = q.Email,
                        }).ToList(),
                        WorkExperiences = student.WorkExperience.Select(q => new WorkExperienceViewModel
                        {
                            WorkExperienceId = q.WorkExperienceId,
                            Employer = q.Employer,
                            Date = q.Date,
                            JobTitle = q.JobTitle,
                            TaskandResponsibilities = q.TaskandResponsibilities,
                        }).ToList()
                    };

                    // Return the full view with the viewModel
                    return View(viewModel);
                }
                else
                {
                    var viewModel = new BuildProfile
                    {
                        StudentProfile = new ManageStudentProfileViewModel
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
                            Department = default,

                            // Non-editable fields
                            FirstName = user.FirstName,
                            Surname = user.Surname,
                            Email = user.Email,
                        },
                        Qualifications = new List<QualificationViewModel>(),
                        Referees = new List<RefereeViewModel>(),
                        WorkExperiences = new List<WorkExperienceViewModel>()
                    };

                    // Return the view with the empty viewModel
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
                        student.Department = viewModel.StudentProfile.Department;

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
                            CareerObjective = viewModel.StudentProfile.CareerObjective,
                            Skills = viewModel.StudentProfile.Skills,
                            Achievements = viewModel.StudentProfile.Achievements,
                            Interests = viewModel.StudentProfile.Interests,
                            IDNumber = viewModel.StudentProfile.IDNumber,
                            Race = viewModel.StudentProfile.SelectedRace,
                            Address = viewModel.StudentProfile.Address,
                            Gender = viewModel.StudentProfile.SelectedGender,
                            DriversLicense = viewModel.StudentProfile.SelectedDriversLicense,
                            Nationality = viewModel.StudentProfile.SelectedNationality,
                            CurrentYearOfStudy = viewModel.StudentProfile.SelectedCurrentYearOfStudy,
                            Faculty = viewModel.StudentProfile.SelectedFaculty,
                            Department = viewModel.StudentProfile.Department
                        };

                        // Add the student to the context
                        _context.Student.Add(student);

                        // Save the changes
                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    }
                    return RedirectToAction("ManageProfile", viewModel);
                }
            }
            else
            {
                // Log ModelState errors for debugging
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            // If the model is not valid, return the same view with the validation errors
            return View("ManageProfile", viewModel);
        }


        [HttpGet]
        public List<string> GetDepartmentss(string faculty)
        {


            List<string> CommerceLawAndManagement = new List<string>() { "Economic And Business Sciences",
                                                                         "Finance And Investment Management",
                                                                         "Industrial Psychology And People Management",
                                                                         "Chemical Engineering",
                                                                         "Law"};



            List<string> EngineeringAndBuiltEnvironment = new List<string>() { "Chemical Engineering",
                                                                               "Civil And Environmental Engineering",
                                                                               "Electrical And Information Engineering",
                                                                               "Mechanical Industrial And Aeronautical Engineering"};
            List<string> HealthSciences = new List<string>() { "Anatomy",
                                                               "Dentistry",
                                                               "Medicine",
                                                               "Pharmacy And Pharmacology"};
            List<string> Humanities = new List<string>() { "Archaeology And Anthropology",
                                                           "Geography Archaeology And Environmental Studies",
                                                           "Political Studies And International Relations"};
            List<string> Science = new List<string>() { "Chemistry",
                                                        "Mathematics",
                                                        "Physics",
                                                        "Zoology And Entomology",
                                                        "Computer Science",
                                                        "Geosciences",
                                                        "Human Physiology",
                                                        "Molecular Medicine And Haematology",
                                                        "School Of Accountancy"};



            if (faculty.Equals("CommerceLawAndManagement"))
            {
                return CommerceLawAndManagement;
            }
            else if (faculty.Equals("EngineeringAndBuiltEnvironment"))



            {
                return EngineeringAndBuiltEnvironment;
            }
            else if (faculty.Equals("HealthSciences"))



            {
                return HealthSciences;
            }
            else if (faculty.Equals("Humanities"))



            {
                return Humanities;
            }
            else
            {
                return Science;
            }

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



        /////////////////QUALIFICATION////////////

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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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


        /////////////////END OF QUALIFICATION////////////



        /////////////////REFEREE////////////


        public IActionResult AddRefereepv()
        {
            var viewModel = new RefereeViewModel
            {
                RefereeId = Guid.NewGuid().ToString(), // Generate a new referee ID,
            };

            return PartialView(viewModel);
        }



        // HTTP POST method to save the addition of a Referee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReferee(RefereeViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {

                    // Create a new Referee entity and map the properties from the view model
                    var newReferee = new Referee
                    {

                        RefereeId = viewModel.RefereeId,
                        FullName = viewModel.FullName,
                        JobTitle = viewModel.JobTitle,
                        Institution = viewModel.Institution,
                        Cellphone = viewModel.Cellphone,
                        Email = viewModel.Email,
                        StudentId = student.StudentId // Set the StudentId using the current student's ID
                    };

                    _context.Referee.Add(newReferee);
                    await _context.SaveChangesAsync();
                    Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    return RedirectToAction("ManageProfile");
                }
            }

            // Handle invalid model state, user, Referee
            return RedirectToAction("ManageProfile");
        }

        public IActionResult EditReferee(string refereeId)
        {
            var referee = _context.Referee.FirstOrDefault(q => q.RefereeId == refereeId);

            if (referee != null)
            {
                var viewModel = new RefereeViewModel
                {
                    RefereeId = referee.RefereeId,
                    FullName = referee.FullName,
                    JobTitle = referee.JobTitle,
                    Institution = referee.Institution,
                    Cellphone = referee.Cellphone,
                    Email = referee.Email,
                };

                return PartialView(viewModel);
            }

            // Handle qualification not found
            return RedirectToAction("ManageProfile");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReferee(RefereeViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {
                    var referee = await _context.Referee.FindAsync(viewModel.RefereeId);

                    if (referee != null)
                    {
                        referee.FullName = viewModel.FullName;
                        referee.JobTitle = viewModel.JobTitle;
                        referee.Institution = viewModel.Institution;
                        referee.Cellphone = viewModel.Cellphone;
                        referee.Email = viewModel.Email;

                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                        return RedirectToAction("ManageProfile");
                    }
                }
            }

            // Handle invalid model state, user, or qualification
            return RedirectToAction("ManageProfile");
        }



        // HTTP POST method to delete a Referee
        [HttpPost]
        public async Task<IActionResult> DeleteReferee(RefereeViewModel viewModel)
        {
            var referee = await _context.Referee.FindAsync(viewModel.RefereeId);

            if (referee != null)
            {
                _context.Referee.Remove(referee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageProfile");
        }


        /////////////////END OF REFEREE////////////



        /////////////////WORK EXPERIENCE////////////


        public IActionResult AddWorkExperiencepv()
        {
            var viewModel = new WorkExperienceViewModel
            {
                WorkExperienceId = Guid.NewGuid().ToString(), // Generate a new work experience ID,
            };

            return PartialView(viewModel);
        }



        // HTTP POST method to save the addition of a work experience
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkExperience(WorkExperienceViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {

                    // Create a new work experience entity and map the properties from the view model
                    var newWorkExpeience = new WorkExperience
                    {

                        WorkExperienceId = viewModel.WorkExperienceId,
                        Employer = viewModel.Employer,
                        Date = viewModel.Date,
                        JobTitle = viewModel.JobTitle,
                        TaskandResponsibilities = viewModel.TaskandResponsibilities,
                        StudentId = student.StudentId // Set the StudentId using the current student's ID
                    };

                    _context.WorkExperience.Add(newWorkExpeience);
                    await _context.SaveChangesAsync();
                    Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                    return RedirectToAction("ManageProfile");
                }
            }

            // Handle invalid model state, user, or workexperience
            return RedirectToAction("ManageProfile");
        }

        public IActionResult EditWorkExperience(string workExperienceId)
        {
            var workExperience = _context.WorkExperience.FirstOrDefault(q => q.WorkExperienceId == workExperienceId);

            if (workExperience != null)
            {
                var viewModel = new WorkExperienceViewModel
                {
                    WorkExperienceId = workExperience.WorkExperienceId,
                    Employer = workExperience.Employer,
                    Date = workExperience.Date,
                    JobTitle = workExperience.JobTitle,
                    TaskandResponsibilities = workExperience.TaskandResponsibilities,
                };

                return PartialView(viewModel);
            }

            // Handle work experience not found
            return RedirectToAction("ManageProfile");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorkExperience(WorkExperienceViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == user.Id);

                if (ModelState.IsValid && student != null)
                {
                    var workExperience = await _context.WorkExperience.FindAsync(viewModel.WorkExperienceId);

                    if (workExperience != null)
                    {
                        workExperience.Employer = viewModel.Employer;
                        workExperience.Date = viewModel.Date;
                        workExperience.JobTitle = viewModel.JobTitle;
                        workExperience.TaskandResponsibilities = viewModel.TaskandResponsibilities;

                        await _context.SaveChangesAsync();
                        Toaster.AddSuccessToastMessage(TempData, "Profile updated successfully.");
                        return RedirectToAction("ManageProfile");
                    }
                }
            }

            // Handle invalid model state, user, or work experience
            return RedirectToAction("ManageProfile");
        }



        // HTTP POST method to delete a Work Experience
        [HttpPost]
        public async Task<IActionResult> DeleteWorkExperience(WorkExperienceViewModel viewModel)
        {
            var workExperience = await _context.WorkExperience.FindAsync(viewModel.WorkExperienceId);

            if (workExperience != null)
            {
                _context.WorkExperience.Remove(workExperience);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageProfile");
        }


        /////////////////END OF WORK EXPERIENCE////////////

        public async Task<IActionResult> StudentApplicationsHistory()
        {

            var studentApplications = new List<StudentApplicationsHistory>();
            var studentApplicationStatus = "";
            var viewmodel = new StudentApplicationsHistory();
            var user = await _userManager.GetUserAsync(User);
            var student = await _context.Student.FindAsync(user.Id);
            var jobPostsIds = await _context.StudentApplication
                        .Where(y => y.StudentId == student.StudentId)
                        .Select(y => y.JobPostId)
                        .ToListAsync();

            foreach (var id in jobPostsIds)
            {
                var jobPost = await _context.JobPost.FindAsync(id);
                var applicationStatus = await _context.StudentApplication
                                        .Where(a => a.StudentId == student.StudentId && a.JobPostId == jobPost.JobPostId)
                                        .Select(a => a.StudentApplicationStatus)
                                        .FirstOrDefaultAsync();
                if (applicationStatus == StudentApplication.EnumStudentApplicationStatus.Withdrawn)
                {
                    studentApplicationStatus = "Withdrawn";
                }
                else if (applicationStatus == StudentApplication.EnumStudentApplicationStatus.Rejected)
                {
                    studentApplicationStatus = "Unsuccessful";
                }
                else if (applicationStatus == StudentApplication.EnumStudentApplicationStatus.Interview)
                {
                    studentApplicationStatus = "Interview";
                }
                else if (applicationStatus == StudentApplication.EnumStudentApplicationStatus.Appointed)
                {
                    studentApplicationStatus = "Scuccessful";
                }
                else if (applicationStatus == StudentApplication.EnumStudentApplicationStatus.OnHold)
                {
                    studentApplicationStatus = "On hold";
                }
                else
                {
                    studentApplicationStatus = "Pending";
                }
                var applicationId = await _context.StudentApplication
                                        .Where(a => a.StudentId == student.StudentId && a.JobPostId == jobPost.JobPostId)
                                        .Select(a => a.ApplicationId)
                                        .FirstOrDefaultAsync();
                var viewModel = new StudentApplicationsHistory
                {
                    StudentApplicationiId = applicationId,
                    JobTitle = jobPost.JobTitle,
                    Department = jobPost.Department,
                    JobType = jobPost.JobType,
                    StartDate = jobPost.StartDate,
                    EndDate = jobPost.EndDate,
                    StudentApplicationStatus = studentApplicationStatus
                };

                studentApplications.Add(viewModel);
            }
            return View(studentApplications);
        }
    }
}
