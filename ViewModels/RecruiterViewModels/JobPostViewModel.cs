using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Data;
using StudentEmploymentPortal.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentEmploymentPortal.ViewModels.RecruiterViewModels
{
    public class JobPostViewModel
    {
        public JobPost.EnumRecruiterType RecruiterType { get; set; }
        public JobPost.EnumFaculty? Faculty { get; set; }
        public string? Department { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public string JobDescription { get; set; }
        public string KeyResponsibilities { get; set; }
        public JobPost.EnumJobType JobType { get; set; }
        public JobPost.EnumWeekHours PartTimeNumberOfHours { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HourlyRate { get; set; }
        public JobPost.EnumNationality Nationality { get; set; }
        public string MinRequirements { get; set; }
        public string ApplicationInstruction { get; set; }
        public DateTime ClosingDate { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public JobPost.EnumApprovalStatus ApprovalStatus { get; set; }
        public bool Approved { get; set; }
        public string? ApproversNote { get; set; }

        // Boolean properties for years of study options
        public bool IsFirstYear { get; set; }
        public bool IsSecondYear { get; set; }
        public bool IsThirdYear { get; set; }
        public bool IsHonours { get; set; }
        public bool IsGraduates { get; set; }
        public bool IsMasters { get; set; }
        public bool IsPhD { get; set; }
        public bool IsPostdoc { get; set; }

        // Dropdown options
        public IEnumerable<SelectListItem> RecruiterTypeOptions => GetEnumSelectList<JobPost.EnumRecruiterType>();
        public IEnumerable<SelectListItem> FacultyOptions => GetEnumSelectList<JobPost.EnumFaculty>();
        //public IEnumerable<SelectListItem> DepartmentOptions => GetEnumSelectList<JobPost.EnumDepartment>();
        public IEnumerable<SelectListItem> WeekHourOptions => GetEnumSelectList<JobPost.EnumWeekHours>();
        public IEnumerable<SelectListItem> JobTypeOptions => GetEnumSelectList<JobPost.EnumJobType>();
        public IEnumerable<SelectListItem> NationalityOptions => GetEnumSelectList<JobPost.EnumNationality>();
        public IEnumerable<SelectListItem> ApprovalStatusOptions => GetEnumSelectList<JobPost.EnumApprovalStatus>();

        public List<string>? YearsOfStudyOptions { get; set; }

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
