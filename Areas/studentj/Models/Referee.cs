using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentj.Models
{
    public class Referee
    {
        [Key]
        public string RefereeId { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; }

        [MaxLength(250)]
        public string JobTitle { get; set; }

        [MaxLength(200)]
        public string Institution { get; set; }

        [RegularExpression(@"^\+27\d{9}$|^\d{10}$", ErrorMessage = "Please enter a valid South African phone number.")]
        public string? Cellphone { get; set; }

        public string? Email { get; set; }

        // Foreign key
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        // Navigation property
        public virtual Student Student { get; set; }

        public Referee()
        {
            FullName = string.Empty;
            JobTitle = string.Empty;
            Institution = string.Empty;
            Cellphone = string.Empty;
            Email = string.Empty;
        }
    }
}
