using StudentEmploymentPortal.Areas.Identity;
using static StudentEmploymentPortal.Areas.studentApplicationJ.Models.StudentApplication;
using static StudentEmploymentPortal.Areas.studentj.Models.Student;

namespace StudentEmploymentPortal.ViewModels.RecruiterViewModels
{
    public class ReviewStudentsApplicationsViewModel
    {
        public string StudentApplicationId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public EnumFaculty Department { get; set; }
        public department Course { get; set; }
        public currentYearOfStudy Level { get; set; }
        public gender Gender { get; set; }
        public string StudentApplicationStatus { get; set; }
    }
}
