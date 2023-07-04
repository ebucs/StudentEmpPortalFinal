using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEmploymentPortal.Models
{
    public class Department
    {
        [Key]
        public string DepartmentId { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("Faculty")]
        public string FacultyId { get; set; }

        public Faculty Faculty { get; set; }


        // Seed data
        public static void ConfigureSeedData(EntityTypeBuilder<Department> builder)
        {
            builder.HasData(
                new Department { DepartmentId = "1", Name = "Economic And Business Sciences", FacultyId = "1" },
                new Department { DepartmentId = "2", Name = "Finance And Investment Management", FacultyId = "1" },
                new Department { DepartmentId = "3", Name = "Industrial Psychology And People Management", FacultyId = "1" },
                new Department { DepartmentId = "4", Name = "Chemical Engineering", FacultyId = "2" },
                new Department { DepartmentId = "5", Name = "Civil And Environmental Engineering", FacultyId = "2" },
                new Department { DepartmentId = "6", Name = "Anatomy", FacultyId = "3" },
                new Department { DepartmentId = "7", Name = "Dentistry", FacultyId = "3" },
                new Department { DepartmentId = "8", Name = "Medicine", FacultyId = "3" },
                new Department { DepartmentId = "9", Name = "Pharmacy And Pharmacology", FacultyId = "3" },
                new Department { DepartmentId = "10", Name = "Archaeology And Anthropology", FacultyId = "4" },
                new Department { DepartmentId = "11", Name = "Geography Archaeology And Environmental Studies", FacultyId = "4" },
                new Department { DepartmentId = "12", Name = "Political Studies And International Relations", FacultyId = "4" },
                new Department { DepartmentId = "13", Name = "Chemistry", FacultyId = "5" },
                new Department { DepartmentId = "14", Name = "Mathematics", FacultyId = "5" },
                new Department { DepartmentId = "15", Name = "Physics", FacultyId = "5" },
                new Department { DepartmentId = "16", Name = "Zoology And Entomology", FacultyId = "5" },
                new Department { DepartmentId = "17", Name = "Computer Science", FacultyId = "5" },
                new Department { DepartmentId = "18", Name = "Geosciences", FacultyId = "5" },
                new Department { DepartmentId = "19", Name = "Human Physiology", FacultyId = "5" },
                new Department { DepartmentId = "20", Name = "Molecular Medicine And Haematology", FacultyId = "5" },
                new Department { DepartmentId = "22", Name = "School Of Accountancy", FacultyId = "5" }
            );
        }
    }
}
