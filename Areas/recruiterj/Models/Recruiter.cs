using Microsoft.AspNetCore.Identity;
using StudentEmploymentPortal.Areas.Identity;
using StudentEmploymentPortal.Areas.jobpostA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.recruiterj.Models
{
    public class Recruiter
    {
        [Key, ForeignKey(nameof(User))]
        public string RecruiterId { get; set; }

        [Display(Name = "Titel")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job title is required.")]
        public string JobTitle { get; set; }

        [Display(Name = "Registration Number")]
        [Required(ErrorMessage = "Registration number is required.")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Registered name is required.")]
        public string RegisteredName { get; set; }

        [Required(ErrorMessage = "Trading name is required.")]
        public string TradingName { get; set; }

        [Required(ErrorMessage = "Business type is required.")]
        public string BusinessType { get; set; }

        [Required(ErrorMessage = "Registered Address is required.")]
        public string RegisteredAddress { get; set; }

        [Display(Name = "Confirm Details")]
        [Required(ErrorMessage ="Must accept that your information is entered correctly")]
        public bool ConfirmDetails { get; set; }

        public bool Approved { get; set; }

        public string? ApproversNote { get; set; }

        public outcomeStatus OutcomeStatus { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; }

        // Collection navigation property for JobPosts
        public virtual ICollection<JobPost> JobPosts { get; set; }

        //constructor

        public Recruiter()
        {
            Title = string.Empty;
            JobTitle = string.Empty;
            RegistrationNumber = string.Empty;
            RegisteredName = string.Empty;
            TradingName = string.Empty;
            BusinessType = string.Empty;
            RegisteredAddress = string.Empty;
            ConfirmDetails = false;
            Approved = false;
            ApproversNote = string.Empty;
            OutcomeStatus = outcomeStatus.Pending;
        }

        public enum outcomeStatus
        {
            Pending,
            Approved,
            Rejected
        }
    }

}
