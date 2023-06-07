using Microsoft.AspNetCore.Identity;
using StudentEmploymentPortal.Areas.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StudentEmploymentPortal.Areas.studentj.Models
{
    public class Student
    {
        [Key, ForeignKey(nameof(User))]
        public string StudentId { get; set; }

        public driversLicense DriversLicense { get; set; }

        public string CareerObjective { get; set; }

        public gender Gender { get; set; }

        public race Race { get; set; }

        public string IDNumber { get; set; }

        public nationality Nationality { get; set; }

        [Required(ErrorMessage = "Current year of study is required.")]
        public currentYearOfStudy CurrentYearOfStudy { get; set; }

        [Required(ErrorMessage = "Faculty is required.")]
        public faculty Faculty { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public department Department { get; set; }

        public string Skills { get; set; }

        public string Achievements { get; set; }

        public string Interests { get; set; }

        public string Address { get; set; }

        // Navigation property
        public virtual ApplicationUser User { get; set; }

        //constructor

        public Student()
        {
            DriversLicense = driversLicense.None;
            CareerObjective = string.Empty;
            Gender = gender.Male;
            Race = race.Black;
            Nationality = nationality.SouthAfrican;
            CurrentYearOfStudy = currentYearOfStudy.FirstYear;
            Faculty = faculty.Science;
            Department = department.ComputerScience;
            Skills = string.Empty;
            Achievements = string.Empty;
            Interests = string.Empty;
            Address = string.Empty;
        }

        //enums
        public enum gender
        {
            [Display(Name = "Male")]
            Male,

            [Display(Name = "Female")]
            Female,

            [Display(Name = "Non-binary")]
            NonBinary
        }

        public enum race
        {
            [Display(Name = "Black")]
            Black,

            [Display(Name = "White")]
            White,

            [Display(Name = "Coloured")]
            Coloured,

            [Display(Name = "Asian")]
            Asian,

            [Display(Name = "Indian")]
            Indian,

            [Display(Name = "Other")]
            Other
        }

        public enum nationality
        {
            [Display(Name = "Algerian")]
            Algerian,

            [Display(Name = "Angolan")]
            Angolan,

            [Display(Name = "Beninese")]
            Beninese,

            [Display(Name = "Botswanan")]
            Botswanan,

            [Display(Name = "Burkinabe")]
            Burkinabe,

            [Display(Name = "Burundian")]
            Burundian,

            [Display(Name = "Cameroonian")]
            Cameroonian,

            [Display(Name = "Cape Verdean")]
            CapeVerdean,

            [Display(Name = "Central African")]
            CentralAfrican,

            [Display(Name = "Chadian")]
            Chadian,

            [Display(Name = "Comoran")]
            Comoran,

            [Display(Name = "Congolese")]
            Congolese,

            [Display(Name = "Djiboutian")]
            Djiboutian,

            [Display(Name = "Egyptian")]
            Egyptian,

            [Display(Name = "Equatorial Guinean")]
            EquatorialGuinean,

            [Display(Name = "Eritrean")]
            Eritrean,

            [Display(Name = "Ethiopian")]
            Ethiopian,

            [Display(Name = "Gabonese")]
            Gabonese,

            [Display(Name = "Gambian")]
            Gambian,

            [Display(Name = "Ghanaian")]
            Ghanaian,

            [Display(Name = "Guinean")]
            Guinean,

            [Display(Name = "Ivorian")]
            Ivorian,

            [Display(Name = "Kenyan")]
            Kenyan,

            [Display(Name = "Lesotho")]
            Basotho,

            [Display(Name = "Liberian")]
            Liberian,

            [Display(Name = "Libyan")]
            Libyan,

            [Display(Name = "Malagasy")]
            Malagasy,

            [Display(Name = "Malawian")]
            Malawian,

            [Display(Name = "Malian")]
            Malian,

            [Display(Name = "Mauritanian")]
            Mauritanian,

            [Display(Name = "Mauritian")]
            Mauritian,

            [Display(Name = "Moroccan")]
            Moroccan,

            [Display(Name = "Mozambican")]
            Mozambican,

            [Display(Name = "Namibian")]
            Namibian,

            [Display(Name = "Nigerian")]
            Nigerian,

            [Display(Name = "Nigerien")]
            Nigerien,

            [Display(Name = "Rwandan")]
            Rwandan,

            [Display(Name = "Sao Tomean")]
            SaoTomean,

            [Display(Name = "Senegalese")]
            Senegalese,

            [Display(Name = "Seychellois")]
            Seychellois,

            [Display(Name = "Sierra Leonean")]
            SierraLeonean,

            [Display(Name = "Somali")]
            Somali,

            [Display(Name = "South African")]
            SouthAfrican,

            [Display(Name = "South Sudanese")]
            SouthSudanese,

            [Display(Name = "Sudanese")]
            Sudanese,

            [Display(Name = "Swazi")]
            Swazi,

            [Display(Name = "Tanzanian")]
            Tanzanian,

            [Display(Name = "Togolese")]
            Togolese,

            [Display(Name = "Tunisian")]
            Tunisian,

            [Display(Name = "Ugandan")]
            Ugandan,

            [Display(Name = "Zambian")]
            Zambian,

            [Display(Name = "Zimbabwean")]
            Zimbabwean
        }

        public enum currentYearOfStudy
        {
            [Display(Name = "1st Year")]
            FirstYear,

            [Display(Name = "2nd Year")]
            SecondYear,

            [Display(Name = "3rd Year")]
            ThirdYear,

            [Display(Name = "Honours")]
            Honours,

            [Display(Name = "Graduates")]
            Graduates,

            [Display(Name = "Masters")]
            Masters,

            [Display(Name = "PhD")]
            PhD,

            [Display(Name = "Postdoc")]
            Postdoc
        }

        public enum faculty
        {
            [Display(Name = "Commerce, Law and Management")]
            CommerceLawAndManagement,

            [Display(Name = "Engineering and the Built Environment")]
            EngineeringAndBuiltEnvironment,

            [Display(Name = "Health Sciences")]
            HealthSciences,

            [Display(Name = "Humanities")]
            Humanities,

            [Display(Name = "Science")]
            Science
        }

        public enum department
        {
            // Commerce, Law and Management
            [Display(Name = "Accounting")]
            Accounting,

            [Display(Name = "Economic and Business Sciences")]
            EconomicAndBusinessSciences,

            [Display(Name = "Finance and Investment Management")]
            FinanceAndInvestmentManagement,

            [Display(Name = "Industrial Psychology and People Management")]
            IndustrialPsychologyAndPeopleManagement,

            [Display(Name = "Law")]
            Law,

            // Engineering and the Built Environment
            [Display(Name = "Chemical Engineering")]
            ChemicalEngineering,

            [Display(Name = "Civil and Environmental Engineering")]
            CivilAndEnvironmentalEngineering,

            [Display(Name = "Electrical and Information Engineering")]
            ElectricalAndInformationEngineering,

            [Display(Name = "Mechanical, Industrial and Aeronautical Engineering")]
            MechanicalIndustrialAndAeronauticalEngineering,

            // Health Sciences
            [Display(Name = "Anatomy")]
            Anatomy,

            [Display(Name = "Dentistry")]
            Dentistry,

            [Display(Name = "Medicine")]
            Medicine,

            [Display(Name = "Pharmacy and Pharmacology")]
            PharmacyAndPharmacology,

            // Humanities
            [Display(Name = "Archaeology and Anthropology")]
            ArchaeologyAndAnthropology,

            [Display(Name = "English")]
            English,

            [Display(Name = "Geography, Archaeology and Environmental Studies")]
            GeographyArchaeologyAndEnvironmentalStudies,

            [Display(Name = "Political Studies and International Relations")]
            PoliticalStudiesAndInternationalRelations,

            // Science
            [Display(Name = "Chemistry")]
            Chemistry,

            [Display(Name = "Mathematics")]
            Mathematics,

            [Display(Name = "Physics")]
            Physics,

            [Display(Name = "Zoology and Entomology")]
            ZoologyAndEntomology,

            // Additional Departments
            [Display(Name = "Computer Science")]
            ComputerScience,

            [Display(Name = "Geosciences")]
            Geosciences,

            [Display(Name = "Human Physiology")]
            HumanPhysiology,

            [Display(Name = "Molecular Medicine and Haematology")]
            MolecularMedicineAndHaematology,

            [Display(Name = "School of Accountancy")]
            SchoolOfAccountancy
        }

        public static class FacultyDepartmentMapping
        {
            public static Dictionary<Student.faculty, List<Student.department>> FacultyDepartments = new Dictionary<Student.faculty, List<Student.department>>()
            {
                // Commerce, Law and Management
                {
                    Student.faculty.CommerceLawAndManagement,
                    new List<Student.department>
                    {
                        Student.department.Accounting,
                        Student.department.EconomicAndBusinessSciences,
                        Student.department.FinanceAndInvestmentManagement,
                        Student.department.IndustrialPsychologyAndPeopleManagement,
                        Student.department.Law
                    }
                },

                // Engineering and the Built Environment
                {
                    Student.faculty.EngineeringAndBuiltEnvironment,
                    new List<Student.department>
                    {
                        Student.department.ChemicalEngineering,
                        Student.department.CivilAndEnvironmentalEngineering,
                        Student.department.ElectricalAndInformationEngineering,
                        Student.department.MechanicalIndustrialAndAeronauticalEngineering
                    }
                },

                // Health Sciences
                {
                    Student.faculty.HealthSciences,
                    new List<Student.department>
                    {
                        Student.department.Anatomy,
                        Student.department.Dentistry,
                        Student.department.Medicine,
                        Student.department.PharmacyAndPharmacology
                    }
                },

                // Humanities
                {
                    Student.faculty.Humanities,
                    new List<Student.department>
                    {
                        Student.department.ArchaeologyAndAnthropology,
                        Student.department.English,
                        Student.department.GeographyArchaeologyAndEnvironmentalStudies,
                        Student.department.PoliticalStudiesAndInternationalRelations
                    }
                },

                // Science
                {
                    Student.faculty.Science,
                    new List<Student.department>
                    {
                        Student.department.Chemistry,
                        Student.department.Mathematics,
                        Student.department.Physics,
                        Student.department.ZoologyAndEntomology,
                        Student.department.ComputerScience,
                        Student.department.Geosciences,
                        Student.department.HumanPhysiology,
                        Student.department.MolecularMedicineAndHaematology
                    }
                }
            };
        }

        public List<department> GetDepartmentsForFaculty(faculty selectedFaculty)
        {
            if (FacultyDepartmentMapping.FacultyDepartments.ContainsKey(selectedFaculty))
            {
                return FacultyDepartmentMapping.FacultyDepartments[selectedFaculty];
            }

            return new List<department>(); // Return an empty list if the faculty is not found
        }



        public enum driversLicense
        {
            [Display(Name = "None")]
            None,

            [Display(Name = "Code A - Motorcycle")]
            CodeA,

            [Display(Name = "Code B - Light motor vehicle")]
            CodeB,

            [Display(Name = "Code C1 - Minibus, bus or heavy vehicle with GVM < 16,000 kg")]
            CodeC1,

            [Display(Name = "Code C - Heavy vehicle with GVM > 16,000 kg")]
            CodeC,

            [Display(Name = "Code EC1 - Articulated heavy vehicle with GVM < 16,000 kg")]
            CodeEC1,

            [Display(Name = "Code EC - Articulated heavy vehicle with GVM > 16,000 kg")]
            CodeEC
        }

    }

}
