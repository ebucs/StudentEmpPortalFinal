using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace StudentEmploymentPortal.Utility
{
    public static class Toaster
    {
        public static void AddSuccessToastMessage(ITempDataDictionary tempData, string message)
        {
            tempData["SuccessMessage"] = message;
        }

        public static void AddErrorToastMessage(ITempDataDictionary tempData, string message)
        {
            tempData["ErrorMessage"] = message;
        }
    }
}
