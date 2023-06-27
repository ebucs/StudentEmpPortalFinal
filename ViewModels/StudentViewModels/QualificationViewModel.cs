using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class QualificationViewModel
    {
        public string QualificationId { get; set; }

        public string Institution { get; set; }

        public string Date { get; set; }

        public string Qualification { get; set; }

        public string? Subjects { get; set; }

        public string? Majors { get; set; }

        public string? Submajors { get; set; }

        public string? Research { get; set; }
    }
}

