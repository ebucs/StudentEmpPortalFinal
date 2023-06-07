using StudentEmploymentPortal.Areas.recruiterj.Models;
namespace StudentEmploymentPortal.ViewModels.RecruiterViewModels
{
    public class CompleteRegisterViewModel
    {
        //Editable fields
        public string Title { get; set; }
        public string JobTitle { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegisteredName { get; set; }
        public string TradingName { get; set; }
        public string BusinessType { get; set; }
        public string RegisteredAddress { get; set; }
        public bool ConfirmDetails { get; set; }
        public bool Approved { get; set; }
        public string PhoneNumber { get; set; }
        public string Telephone { get; set; }

        // Non-editable fields
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
       
    }
}
