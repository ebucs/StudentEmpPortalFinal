using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.jobpostA.Models
{
    public class YearsOfStudy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string YearsOfStudyId { get; set; } // Primary key

        // Foreign key property
        [ForeignKey("JobPost")]
        public string JobPostId { get; set; }

        public virtual JobPost JobPost { get; set; }

        [Required(ErrorMessage = "First year is required.")]
        public bool IsFirstYear { get; set; }

        [Required(ErrorMessage = "Second year is required.")]
        public bool IsSecondYear { get; set; }

        [Required(ErrorMessage = "Third year is required.")]
        public bool IsThirdYear { get; set; }

        [Required(ErrorMessage = "Honours is required.")]
        public bool IsHonours { get; set; }

        [Required(ErrorMessage = "Graduates is required.")]
        public bool IsGraduates { get; set; }

        [Required(ErrorMessage = "Masters is required.")]
        public bool IsMasters { get; set; }

        [Required(ErrorMessage = "PhD is required.")]
        public bool IsPhD { get; set; }

        [Required(ErrorMessage = "Postdoc is required.")]
        public bool IsPostdoc { get; set; }
    }

}
