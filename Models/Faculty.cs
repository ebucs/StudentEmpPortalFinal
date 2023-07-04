using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace StudentEmploymentPortal.Models
{
    public class Faculty
    {
        [Key]
        public string FacultyId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }


        // Seed data
        public static void ConfigureSeedData(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasData(
                new Faculty { FacultyId = "1", Name = "Commerce, Law and Management" },
                new Faculty { FacultyId = "2", Name = "Engineering and the Built Environment" },
                new Faculty { FacultyId = "3", Name = "Health Sciences" },
                new Faculty { FacultyId = "4", Name = "Humanities" },
                new Faculty { FacultyId = "5", Name = "Science" }
            );
        }
    }
}
