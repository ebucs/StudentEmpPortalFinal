using Microsoft.AspNetCore.Mvc.Rendering;
using StudentEmploymentPortal.Areas.recruiterj.Models;
using StudentEmploymentPortal.Areas.studentj.Models;

using StudentEmploymentPortal.Utility;

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

        // Additional properties for the approver
        public string? ApproverNote { get; set; }
        public Recruiter.outcomeStatus Outcome { get; set; }

        public IEnumerable<SelectListItem> StatusOptions => GetEnumSelectList<Recruiter.outcomeStatus>();

        // Helper method to populate dropdown options from enum values
        private static IEnumerable<SelectListItem> GetEnumSelectList<T>() where T : struct, Enum
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
