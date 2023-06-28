using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Areas.studentApplicationJ.Models
{
    public class StudentApplication
    {
        //ApplicationId
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ApplicationId { get; set; }

        //JobPostId
        [ForeignKey("JobPostId")]
        public string JobPostId { get; set; }

        //StudentId
        [ForeignKey("StudentId")]
        public string StudentId { get; set; }

        //RecruiterId
        [ForeignKey("RecruiterId")]
        public string RecruiterId { get; set; }

        // DateCreated
        public DateTime DateCreated { get; set; }

        //DateUpdated

        public DateTime ReviewDate { get; set; }

        //Status
        public EnumStudentApplicationStatus StudentApplicationStatus { get; set; }

        public bool IsWithdrawn { get; set; }

        // ApplicationDocument
        //[Required]
        //public byte[] ApplicationDocument { get; set; }


        public StudentApplication()
        {
            StudentApplicationStatus = EnumStudentApplicationStatus.OnHold;
            DateCreated = DateTime.Now;
            ReviewDate = DateTime.Now;
        }
        //enums
        public enum EnumStudentApplicationStatus
        {
            InterView,
            OnHold,
            Rejected,
            Appointed,
            Withdrawn,
            Pending
        }
    }
}