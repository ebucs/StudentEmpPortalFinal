using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.studentj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using StudentEmploymentPortal.Utility;
using StudentEmploymentPortal.Models;

namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class ManageStudentProfileViewModel
    {
        // Editable fields
        public string? CareerObjective { get; set; }
        public string? Skills { get; set; }
        public string? Achievements { get; set; }
        public string? Interests { get; set; }
        public string IDNumber { get; set; }
        public Student.race SelectedRace { get; set; }
        public Student.gender SelectedGender { get; set; }
        public Student.driversLicense SelectedDriversLicense { get; set; }
        public Student.EnumNationality SelectedNationality { get; set; }
        public Student.currentYearOfStudy SelectedCurrentYearOfStudy { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        // Non-editable fields
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string? Address { get; set; }

        // Dropdown options
        public IEnumerable<SelectListItem> DriversLicenseOptions => GetEnumSelectList<Student.driversLicense>();
        public IEnumerable<SelectListItem> GenderOptions => GetEnumSelectList<Student.gender>();
        public IEnumerable<SelectListItem> RaceOptions => GetEnumSelectList<Student.race>();
        public IEnumerable<SelectListItem> NationalityOptions => GetEnumSelectList<Student.EnumNationality>();
        public IEnumerable<SelectListItem> CurrentYearOfStudyOptions => GetEnumSelectList<Student.currentYearOfStudy>();
        public IEnumerable<SelectListItem> FacultyOptions => GetEnumSelectList<Student.EnumFaculty>();
        public IEnumerable<SelectListItem> DepartmentOptions => GetEnumSelectList<Student.department>();

        // Helper method to populate dropdown options from enum values
        private IEnumerable<SelectListItem> GetEnumSelectList<T>() where T : struct, Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDisplayName()
                });
        }
    }
}
