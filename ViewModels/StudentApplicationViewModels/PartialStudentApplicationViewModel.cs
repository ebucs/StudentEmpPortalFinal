using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Utility;

namespace StudentEmploymentPortal.ViewModels.StudentApplicationViewModels
{
    public class PartialStudentApplicationViewModel
    {
        //StudentApplicationId
        public string StudentApplicationId { get; set; }

        //JobPost
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
        public string SelectedStudentApplicationStatus { get; set; }

        public enum EnumOutcomeStudentApplicationStatus
        {
            Interview,
            OnHold,
            Rejected,
            Appointed
        }

        public IEnumerable<SelectListItem> OutcomeOptions => GetEnumSelectList<EnumOutcomeStudentApplicationStatus>();


        // Helper method to populate dropdown options from enum values
        private IEnumerable<SelectListItem> GetEnumSelectList<T>() where T : struct, Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDisplayName()
                });
        }
    }
}