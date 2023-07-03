using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using StudentEmploymentPortal.ViewModels.StudentApplicationViewModels;
using StudentEmploymentPortal.ViewModels.StudentViewModels;
using static StudentEmploymentPortal.Areas.studentApplicationJ.Models.StudentApplication;

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

        public async Task<IActionResult> ReviewStudentApplications(string jobPostId)
        {
            var user = await _userManager.GetUserAsync(User);
            var recruiter = await _context.Recruiter.FindAsync(user.Id);
            var studentApplicationStatus = "";

            if (recruiter != null)
            {
                var studentApplications = await _context.StudentApplication
                    .Where(r => r.RecruiterId == recruiter.RecruiterId && r.JobPostId == jobPostId)
                    .ToListAsync();

                var viewModelList = new List<ReviewStudentsApplicationsViewModel>();

                foreach (var studentApplication in studentApplications)
                {
                    var student = await _context.Student
                        .Include(s => s.User)
                        .FirstOrDefaultAsync(s => s.StudentId == studentApplication.StudentId);

                    if (student != null)
                    {
                        if (studentApplication.StudentApplicationStatus == EnumStudentApplicationStatus.Withdrawn)
                        {
                            studentApplicationStatus = "Withdrawn";
                        }
                        else if (studentApplication.StudentApplicationStatus == EnumStudentApplicationStatus.Rejected)
                        {
                            studentApplicationStatus = "Unsuccessful";
                        }
                        else if (studentApplication.StudentApplicationStatus == EnumStudentApplicationStatus.Interview)
                        {
                            studentApplicationStatus = "Interview";
                        }
                        else if (studentApplication.StudentApplicationStatus == EnumStudentApplicationStatus.Appointed)
                        {
                            studentApplicationStatus = "Successful";
                        }
                        else if (studentApplication.StudentApplicationStatus == EnumStudentApplicationStatus.OnHold)
                        {
                            studentApplicationStatus = "On hold";
                        }
                        else
                        {
                            studentApplicationStatus = "Pending";
                        }

                        //ViewModel
                        var viewModel = new ReviewStudentsApplicationsViewModel
                        {
                            StudentApplicationId = studentApplication.ApplicationId,
                            FirstName = student.User.FirstName,
                            Surname = student.User.Surname,
                            Department = student.Faculty,
                            Course = student.Department,
                            Level = student.CurrentYearOfStudy,
                            Gender = student.Gender,
                            StudentApplicationStatus = studentApplicationStatus
                        };

                        viewModelList.Add(viewModel);
                    }
                }

                return View(viewModelList);
            }

            return NotFound();
        }


        public async Task<IActionResult> PartialRecruiterStudentApplicationReview(string id)
        {
            var studentId = await _context.StudentApplication
                       .Where(y => y.ApplicationId == id)
                       .Select(y => y.StudentId)
                       .FirstOrDefaultAsync();
            var student = await _context.Student.FindAsync(studentId);
            var studentApplication = await _context.StudentApplication.FirstOrDefaultAsync(r => r.StudentId == studentId);
            if (studentApplication == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(studentId);
            if (user == null)
            {
                return NotFound();
            }
            if (student == null)
            {
                return NotFound();
            }
            var jobPostId = await _context.StudentApplication
                       .Where(y => y.ApplicationId == id)
                       .Select(y => y.JobPostId)
                       .FirstOrDefaultAsync();
            var jobpost = await _context.JobPost.FindAsync(jobPostId);

            if (jobpost == null)
            {
                return NotFound();
            }

            var qualifications = await _context.Qualification
                .Where(q => q.StudentId == studentId)
                .ToListAsync();

            var referees = await _context.Referee
                .Where(q => q.StudentId == studentId)
                .ToListAsync();

            var workExperiences = await _context.WorkExperience
                .Where(q => q.StudentId == studentId)
                .ToListAsync();

            // Fetch the document names and file paths
            var documents = await _context.ApplicationDocument
                .Where(d => d.StudentApplicationId == id)
                .Select(d => new StudentApplicationDocsViewModel
                {
                    DocumentName = d.DocumentName,
                    FilePath = d.FilePath
                })
                .ToListAsync();


            var yearsOfStudy = await _context.YearsOfStudy.FirstOrDefaultAsync(y =>
                  y.JobPostId == jobpost.JobPostId &&
                  (y.IsFirstYear || y.IsSecondYear || y.IsThirdYear || y.IsHonours || y.IsGraduates || y.IsMasters || y.IsPhD || y.IsPostdoc));

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

            var qualificationViewModels = new List<QualificationViewModel>();

            foreach (var qualification in qualifications)
            {
                var qualificationViewModel = new QualificationViewModel
                {
                    Institution = qualification.Institution,
                    Date = qualification.Date,
                    Qualification = qualification.StuQualification,
                    Majors = qualification.Majors
                };

                qualificationViewModels.Add(qualificationViewModel);
            }

            var refereViewModels = new List<RefereeViewModel>();

            foreach (var referee in referees)
            {
                var refereeViewModel = new RefereeViewModel
                {
                    FullName =  referee.FullName,
                    JobTitle = referee.JobTitle,
                    Institution =  referee.Institution,
                    Email = referee.Email
                };
                refereViewModels.Add(refereeViewModel);
            }

            var workExperienceViewModels = new List<WorkExperienceViewModel>();

            foreach (var workexperience in workExperiences)
            {
                var workExpViewModel = new WorkExperienceViewModel
                {
                    Employer = workexperience.Employer,
                    Date = workexperience.Date,
                    JobTitle = workexperience.JobTitle
                };

                workExperienceViewModels.Add(workExpViewModel);
            }


            var viewmodel = new PartialStudentApplicationViewModel
            {
                StudentApplicationId = id,
                JobTitle = jobpost.JobTitle,
                JobDescription = jobpost.JobDescription,
                Department = jobpost.Faculty.ToString(),
                Course = jobpost.Department.ToString(),
                Levels = yearsOfStudyOptions,

                FirstName = user.FirstName,
                Surname = user.Surname,
                Address = student.Address,
                CareerObjective = student.CareerObjective,
                Skills = student.Skills,
                Achievements = student.Achievements,
                Interests = student.Interests,
                IDNumber = student.IDNumber,
                Race = student.Race.ToString(),
                Gender = student.Gender.ToString(),
                DriversLicense = student.DriversLicense.ToString(),
                Nationality = student.Nationality.ToString(),
                CurrentYearOfStudy = student.CurrentYearOfStudy.ToString(),
                PhoneNumber = user.PhoneNumber,
                Telephone = user.Telephone,
                Email = user.Email,
                SelectedStudentApplicationStatus = studentApplication.StudentApplicationStatus,

                Qualifications = qualificationViewModels,
                Referees = refereViewModels,
                WorkExperiences = workExperienceViewModels,

                DocumentViewModels = documents,
                jobPostId = jobPostId

            };

            return PartialView(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartialRecruiterStudentApplicationReview(string id, PartialStudentApplicationViewModel viewmodel)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studenApplication = await _context.StudentApplication.FirstOrDefaultAsync(r => r.ApplicationId == id );

            if (studenApplication == null)
            {
                return NotFound();
            }

            studenApplication.StudentApplicationStatus = viewmodel.SelectedStudentApplicationStatus;



            //// Update the Approved property based on the Outcome
            //if (viewmodel.SelectedStudentApplicationStatus == "Approved")
            //{
            //    studenApplication.StudentApplicationStatus = true;
            //}
            //else
            //{
            //    recruiter.Approved = false;
            //}

            // Save the changes to the database
            await _context.SaveChangesAsync();
            Toaster.AddSuccessToastMessage(TempData, "Student Application Status Updated.");
            return RedirectToAction("ReviewStudentApplications", new {viewmodel.jobPostId});
        }


    }
}
