using StudentEmploymentPortal.Areas.studentApplicationJ.Models;
using StudentEmploymentPortal.Areas.studentj.Models;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class StudentApplicationDocumentViewModel
    {
        public Student student { get; set; }
        public byte[] Document { get; set; }
    }
}
