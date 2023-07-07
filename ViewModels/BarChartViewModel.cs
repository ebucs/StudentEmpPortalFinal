namespace StudentEmploymentPortal.ViewModels
{
    public class BarChartViewModel
    {
        public List<string> RecruiterTypeLabels { get; set; }
        public List<int> RecruiterTypeData { get; set; }
        public List<string> HourlyRatesLabels { get; set; }
        public List<int> HourlyRatesData { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
