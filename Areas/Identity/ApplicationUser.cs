using Microsoft.AspNetCore.Identity;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentj.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentEmploymentPortal.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Custom properties

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid telephone number.")]
        public string Telephone { get; set; }

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Recruiter Recruiter { get; set; }
    }
}

