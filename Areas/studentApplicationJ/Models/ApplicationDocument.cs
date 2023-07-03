using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Models
{
    public class ApplicationDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ApplicationDocumentId { get; set; }

        [Required]
        public string DocumentName { get; set; }

        public string FilePath { get; set; }

        public string StudentApplicationId { get; set; }

        [ForeignKey(nameof(StudentApplicationId))]
        public virtual StudentApplication StudentApplication { get; set; }
    }
}