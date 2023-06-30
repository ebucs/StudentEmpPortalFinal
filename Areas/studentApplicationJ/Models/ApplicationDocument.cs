using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Models
{
    public class ApplicationDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ApplicationDocumentId { get; set; }

        [ForeignKey("ApplicationId")]
        public string ApplicationId { get; set; }

        [Required]
        public string DocumentName { get; set; }

        public string FilePath { get; set; }
    }
}