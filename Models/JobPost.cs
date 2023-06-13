using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Models
{
    public class JobPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobPostId { get; set; }

        [ForeignKey("RecruiterId")]
        [Required]
        public String RecruiterId { get; set; }

        [Required(ErrorMessage = "InternalOrExternal is required.")]
        public EnumInternalOrExternal InternalOrExternal { get; set; }
        [Required(ErrorMessage = "Faculty field is required.")]
        public String Faculty { get; set; }
        [Required(ErrorMessage = "Department field is required.")]
        public String Department { get; set; }
        [Required(ErrorMessage = "JobTitle field is required.")]
        public String JobTitle { get; set; }
        [Required(ErrorMessage = "Location field is required.")]
        public String Location { get; set; }
        [Required(ErrorMessage = "Job Decription field is required.")]
        public String JobDecription { get; set; }
        [Required(ErrorMessage = "Key Responsibilities field is required.")]
        public String KeyResponsibilities { get; set; }
        [Required(ErrorMessage = "Full time or Part time field is required.")]
        public EnumFullTimeOrPartTime FullTimeOrPartTime { get; set; }
        [Required(ErrorMessage = "Part time number of hours field is required.")]
        public EnumPartTimeNumberOfHours PartTimeNumberOfHours { get; set; }
        [Required(ErrorMessage = "Start date field is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date field is required.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Hourly Rate field is required.")]
        public String HourlyRate { get; set; }
        [Required(ErrorMessage = "Years of Study field is required.")]
        public EnumYearsOfStudy YearsOfStudy { get; set; }
        [Required(ErrorMessage = "Nationality field is required.")]
        public EnumNationality Nationality { get; set; }
        [Required(ErrorMessage = "Minimum Requirements field is required.")]
        public String MinRequirements { get; set; }
        [Required(ErrorMessage = "Application Instruction field is required.")]
        public String ApplicationInstruction { get; set; }
        [Required(ErrorMessage = "Closing date field is required.")]
        public DateTime ClosingDate { get; set; }
        [Required(ErrorMessage = "Contact Person field is required.")]
        public String ContactPerson { get; set; }
        [Required(ErrorMessage = "Email field is required.")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Contact No field is required.")]
        public String ContactNo { get; set; }
        public EnumApprovalStatus? ApprovalStatus { get; set; }

        public JobPost()
        {
            RecruiterId= string.Empty;
            InternalOrExternal = EnumInternalOrExternal.Internal;
            Faculty = string.Empty;
            Department = string.Empty;
            JobTitle = string.Empty;
            Location = string.Empty;
            JobDecription = string.Empty;
            KeyResponsibilities = string.Empty;
            FullTimeOrPartTime = EnumFullTimeOrPartTime.FullTime;
            PartTimeNumberOfHours = EnumPartTimeNumberOfHours.LessThanTwo;
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            HourlyRate = string.Empty;
            YearsOfStudy = EnumYearsOfStudy.FirstYear;
            Nationality = EnumNationality.SouthAfricanCitizens;
            MinRequirements = string.Empty;
            ApplicationInstruction = string.Empty;
            ClosingDate = DateTime.Now.Date;
            ContactPerson = string.Empty;
            Email = string.Empty;
            ContactNo = string.Empty;
           
        }

        // Enums

        public enum EnumInternalOrExternal
        {
            Internal,
            External
        }
        public enum EnumPartTimeNumberOfHours
        {
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
        public enum EnumFullTimeOrPartTime
        {
            [Display(Name = "Full time")]
            FullTime,
            [Display(Name = "Part time")]
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
           
            [Display(Name = "South African citizens")]
            SouthAfricanCitizens,

            [Display(Name = "Open to everyone")]
            OpenToEveryone
        }

        public enum EnumYearsOfStudy
        {
            [Display(Name = "1st Year")]
            FirstYear,

            [Display(Name = "2nd Year")]
            SecondYear,

            [Display(Name = "3rd Year")]
            ThirdYear,

            [Display(Name = "Honours")]
            Honours,

            [Display(Name = "Graduates")]
            Graduates,

            [Display(Name = "Masters")]
            Masters,

            [Display(Name = "PhD")]
            PhD,

            [Display(Name = "Postdoc")]
            Postdoc
        }

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
