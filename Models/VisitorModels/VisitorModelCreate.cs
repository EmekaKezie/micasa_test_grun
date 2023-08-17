namespace TheEstate.Models.VisitorModels
{
    public class VisitorModelCreate : VisitorModel
    {
        public string? ValidityStartDate { get; set; }
        public string? ValidityEndDate { get; set; }
        public string? AccessStartTime { get; set; }
        public string? AccessEndTime { get; set; }
        public List<string>? RecurringDays { get; set; }
    }
}
