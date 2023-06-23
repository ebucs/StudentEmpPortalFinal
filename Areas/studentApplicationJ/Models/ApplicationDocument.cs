using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Models
{
    public class ApplicationDocument
    {
        //ApplicationId
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ApplicationDocumentId { get; set; }

        //ApplicationId
        [ForeignKey("ApplicationId")]
        public string ApplicationId { get; set; }

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public byte[] Documet { get; set; }

    }
}
