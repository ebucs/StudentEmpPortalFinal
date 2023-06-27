namespace StudentEmploymentPortal.ViewModels.StudentViewModels
{
    public class BuildProfile
    {
        public ManageStudentProfileViewModel StudentProfile { get; set; }
        public List<QualificationViewModel>? Qualifications { get; set; }
        public List<RefereeViewModel>? Referees { get; set; }
        public List<WorkExperienceViewModel>? WorkExperiences { get; set; }
    }
}
