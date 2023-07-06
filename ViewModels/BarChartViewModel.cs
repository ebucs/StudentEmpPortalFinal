namespace StudentEmploymentPortal.ViewModels
{
    public class BarChartViewModel
    {
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
