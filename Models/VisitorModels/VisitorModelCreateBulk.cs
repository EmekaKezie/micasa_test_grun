namespace TheEstate.Models.VisitorModels
{
    public class VisitorModelCreateBulk
    {
        public string? BookingTitle { get; set; }
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        public string? ResidentId { get; set; }
        public string? PassageMode { get; set; } //SINGLE, MULTIPLE
        public string? IsRecurring { get; set; }
        public string? IsExitClearanceRequired { get; set; }
        public string? ValidityStartDate { get; set; }
        public string? ValidityEndDate { get; set; }
        public string? AccessStartTime { get; set; }
        public string? AccessEndTime { get; set; }
        public List<string>? RecurringDays { get; set; }
        //public List<VisitorModelBulkCompanion>? Visitor { get; set; }
        //public List<string>? Visitor { get; set; }
        public string? Visitor { get; set; }
    }

    public class VisitorModelBulkCompanion
    {
        public string? VisitorFullname { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorEmail { get; set; }
        public string? IsAccompanied { get; set; }
        public string? VisitorCompanions { get; set; }
        public string? QrCodeText { get; set; }
    }


    public class VisitorModelBulkCompanionKey
    {
        public List<VisitorModelBulkCompanion>? Key { get; set; }
    }












    public class VisitorModelCreateBulk2
    {
        public string? BookingTitle { get; set; }
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        public string? ResidentId { get; set; }
        public string? PassageMode { get; set; } //SINGLE, MULTIPLE
        public string? IsRecurring { get; set; }
        public string? IsExitClearanceRequired { get; set; }
        public string? ValidityStartDate { get; set; }
        public string? ValidityEndDate { get; set; }
        public string? AccessStartTime { get; set; }
        public string? AccessEndTime { get; set; }
        public List<string>? RecurringDays { get; set; }
        //public List<VisitorModelBulkCompanion>? Visitor { get; set; }
        public string? Visitor { get; set; }
        //public VisitorModelBulkCompanionKey? Visitor { get; set; }
    }
}
