using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Utility;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class StudentJobPostViewModel
    {
        public string JobPostId { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string KeyResponsibilities { get; set; }
        public JobPost.EnumJobType JobType { get; set; }
        public JobPost.EnumWeekHours PartTimeNumberOfHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HourlyRate { get; set; }
        public string MinRequirements { get; set; }
        public string ApplicationInstruction { get; set; }
        public DateTime ClosingDate { get; set; }


        // Dropdown options
        public IEnumerable<SelectListItem> WeekHourOptions => GetEnumSelectList<JobPost.EnumWeekHours>();
        public IEnumerable<SelectListItem> JobTypeOptions => GetEnumSelectList<JobPost.EnumJobType>();

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
