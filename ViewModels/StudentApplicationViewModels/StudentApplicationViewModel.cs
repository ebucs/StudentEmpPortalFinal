using StudentEmploymentPortal.Areas.jobpostA.Models;
using StudentEmploymentPortal.Areas.studentj.Models;

namespace StudentEmploymentPortal.ViewModels.StudentApplicationViewModels
{
    public class StudentApplicationViewModel
    {
        public string studentApplicationId { get; set; }
        public JobPost jobPost { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public Student student { get; set; }
    }
}
