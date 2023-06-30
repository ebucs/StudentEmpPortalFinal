using static StudentEmploymentPortal.Areas.jobpostA.Models.JobPost;
using StudentEmploymentPortal.Areas.studentj.Models;
using static StudentEmploymentPortal.Areas.studentApplicationJ.Models.StudentApplication;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class StudentApplicationsHistory
    {
        public string StudentApplicationiId { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public EnumJobType JobType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StudentApplicationStatus { get; set; }
    }
}
