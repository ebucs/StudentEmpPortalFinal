using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.studentj.Models;
using StudentEmploymentPortal.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentEmploymentPortal.Areas.recruiterj.Models;


namespace StudentEmploymentPortal.Areas.jobpostA.Models
{
    public class JobPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string JobPostId { get; set; }

        // Foreign key property
        public string RecruiterId { get; set; }

        // Navigation property for the Recruiter
        [ForeignKey("RecruiterId")]
        public virtual Recruiter Recruiter { get; set; }

        // Add navigation property for YearsOfStudy
        public virtual YearsOfStudy YearsOfStudy { get; set; }

        [Required(ErrorMessage = "Internal Or External is required.")]
        public EnumRecruiterType RecruiterType { get; set; }

        public EnumFaculty Faculty { get; set; }

        public string Department { get; set; }

        [Required(ErrorMessage = "Job Title field is required.")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Location field is required.")]
        public string Location { get; set; }

        [Display(Name = "Job Description")]
        [Required(ErrorMessage = "Job Description field is required.")]
        public string JobDescription { get; set; }

        [Required(ErrorMessage = "Key Responsibilities field is required.")]
        public string KeyResponsibilities { get; set; }

        [Required(ErrorMessage = "Full time or Part time field is required.")]
        public EnumJobType JobType { get; set; }

        [Required(ErrorMessage = "Part time number of hours field is required.")]
        public EnumWeekHours? PartTimeNumberOfHours { get; set; }

        [Required(ErrorMessage = "Start date field is required.")]
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date field is required.")]
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Hourly Rate field is required.")]
        public int HourlyRate { get; set; }

        [Required(ErrorMessage = "Nationality field is required.")]
        public EnumNationality Nationality { get; set; }

        [Required(ErrorMessage = "Minimum Requirements field is required.")]
        public string MinRequirements { get; set; }

        [Required(ErrorMessage = "Application Instruction field is required.")]
        public string ApplicationInstruction { get; set; }

        [Required(ErrorMessage = "Closing date field is required.")]
        [Column(TypeName = "Date")]
        public DateTime ClosingDate { get; set; }

        [Required(ErrorMessage = "Contact Person field is required.")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No field is required.")]
        public string ContactNo { get; set; }

        public EnumApprovalStatus ApprovalStatus { get; set; }

        public bool Approved { get; set; }

        public string? ApproversNote { get; set; }


        public JobPost()
        {
            RecruiterType = EnumRecruiterType.Internal;
            JobTitle = string.Empty;
            Location = string.Empty;
            JobDescription = string.Empty;
            KeyResponsibilities = string.Empty;
            //JobType = EnumJobType.FullTime;
            //PartTimeNumberOfHours = EnumWeekHours.None;
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            HourlyRate = 0;
            //Nationality = EnumNationality.SouthAfrican;
            MinRequirements = string.Empty;
            ApplicationInstruction = string.Empty;
            ClosingDate = DateTime.Now.Date;
            ContactPerson = string.Empty;
            Email = string.Empty;
            ContactNo = string.Empty;
            Approved = false;
            ApproversNote = string.Empty;
            ApprovalStatus = EnumApprovalStatus.Pending;
        }

        // Enums

        public enum EnumRecruiterType
        {
            Internal,
            External
        }
        public enum EnumWeekHours
        {
            None,
            [Display(Name = "<2")]
            LessThanTwo,
            [Display(Name = "2 to 4")]
            TwoToFour,
            [Display(Name = "4 to 6")]
            FourToSix,
            [Display(Name = "6 to 8")]
            SixToEight,
            [Display(Name = "8 to 12")]
            EightToTwelve,
            [Display(Name = ">12")]
            LessThanTwelve,
        }
        public enum EnumJobType
        {
            [Display(Name = "Full-Time")]
            FullTime,
            [Display(Name = "Part-Time")]
            PartTime
        }
        public enum EnumApprovalStatus
        {
            Pending,
            Queried,
            Rejected,
            Approved,
            Closed,
            Withdrawn
        }


        public enum EnumNationality
        {

            [Display(Name = "South African")]
            SouthAfrican,

            [Display(Name = "Open to everyone")]
            OpenToEveryone
        }


        public enum EnumFaculty
        {
            NA,
            [Display(Name = "Commerce, Law and Management")]
            CommerceLawAndManagement,

            [Display(Name = "Engineering and the Built Environment")]
            EngineeringAndBuiltEnvironment,

            [Display(Name = "Health Sciences")]
            HealthSciences,

            [Display(Name = "Humanities")]
            Humanities,

            [Display(Name = "Science")]
            Science
        }

    }
}