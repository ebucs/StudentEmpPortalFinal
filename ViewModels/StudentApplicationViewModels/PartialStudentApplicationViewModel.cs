using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.ViewModels.RecruiterViewModels;
using StudentEmploymentPortal.ViewModels.StudentViewModels;
using static StudentEmploymentPortal.Areas.studentApplicationJ.Models.StudentApplication;

namespace StudentEmploymentPortal.ViewModels.StudentApplicationViewModels
{
    public class PartialStudentApplicationViewModel
    {
        //StudentApplicationId
        public string StudentApplicationId { get; set; }

        //JobPost
        public string jobPostId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Department { get; set; }
        public string Course { get; set; }
        public List<string> Levels { get; set; }

        //Student

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string CareerObjective { get; set; }
        public string Skills { get; set; }
        public string Achievements { get; set; }
        public string Interests { get; set; }
        public string IDNumber { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string DriversLicense { get; set; }
        public string Nationality { get; set; }
        public string CurrentYearOfStudy { get; set; }
        public string PhoneNumber { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public EnumStudentApplicationStatus SelectedStudentApplicationStatus { get; set; }

        //Qualification
        public List<QualificationViewModel> Qualifications { get; set; }

        //Referee
        public List<RefereeViewModel> Referees { get; set; }

        //Work Experience
        public List<WorkExperienceViewModel> WorkExperiences { get; set; }

        //Documents
        public List<StudentApplicationDocsViewModel> DocumentViewModels { get; set; }


        public IEnumerable<SelectListItem> OutcomeOptions => GetFilteredEnumSelectList<EnumStudentApplicationStatus>();

        private IEnumerable<SelectListItem> GetFilteredEnumSelectList<TEnum>() where TEnum : Enum
        {
            var filteredValues = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Where(e => e.Equals(EnumStudentApplicationStatus.Interview) ||
                            e.Equals(EnumStudentApplicationStatus.OnHold) ||
                            e.Equals(EnumStudentApplicationStatus.Rejected) ||
                            e.Equals(EnumStudentApplicationStatus.Appointed));

            return filteredValues.Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDisplayName()
            });
        }




        //// Helper method to populate dropdown options from enum values
        //private IEnumerable<SelectListItem> GetEnumSelectList<T>() where T : struct, Enum
        //{
        //    return Enum.GetValues(typeof(T))
        //        .Cast<T>()
        //        .Select(e => new SelectListItem
        //        {
        //            Value = e.ToString(),
        //            Text = e.GetDisplayName()
        //        });
        //}
    }
}