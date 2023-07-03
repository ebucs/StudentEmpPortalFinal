using StudentEmploymentPortal.Areas.studentj.Models;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class StudentApplicationDocumentViewModel
    {
        public Student student { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}
