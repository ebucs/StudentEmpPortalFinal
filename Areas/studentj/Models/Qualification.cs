using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentj.Models
{
    public class Qualification
    {
        [Key]
        public string QualificationId { get; set; }

        [MaxLength(100)]
        public string Institution { get; set; }

        public string Date { get; set; }

        [MaxLength(100)]
        public string StuQualification { get; set; }

        public string? Subjects { get; set; }

        public string? Majors { get; set; }

        public string? Submajors { get; set; }

        public string? Research { get; set; }

        // Foreign key
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        // Navigation property
        public virtual Student Student { get; set; }

        // Constructor
        public Qualification()
        {
            Institution = string.Empty;
            Date = string.Empty;
            StuQualification = string.Empty;
            Subjects = string.Empty;
            Majors = string.Empty;
            Submajors = string.Empty;
            Research = string.Empty;
        }
    }
}
