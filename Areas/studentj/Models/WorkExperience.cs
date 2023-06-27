using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentj.Models
{
    public class WorkExperience
    {
        [Key]
        public string WorkExperienceId { get; set; }

        public string? Employer { get; set; }

        public string? Date { get; set; }

        public string? JobTitle { get; set; }

        public string? TaskandResponsibilities { get; set; }

        // Foreign key
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        // Navigation property
        public virtual Student Student { get; set; }

        public WorkExperience()
        {
            Employer = string.Empty;
            Date = string.Empty;
            JobTitle = string.Empty;
            TaskandResponsibilities = string.Empty;
        }
    }
}
