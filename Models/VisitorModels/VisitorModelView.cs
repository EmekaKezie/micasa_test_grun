namespace TheEstate.Models.VisitorModels
{
    public class VisitorModelView : VisitorModel
    {
        public string? BookingId { get; set; }
        public string? ZoneName { get; set; }
        public string? EstateName { get; set; }
        public string? EstateDesc { get; set; }
        public string? ResidentEmail { get; set; }
        public string? ResidentMobileNo { get; set; }
        public string? ResidentFirstname { get; set; }
        public string? ResidentLastname { get; set; }
        public string? AccessCode { get; set; }
        public string? RecurringMonday { get; set; }
        public string? RecurringTuesday { get; set; }
        public string? RecurringWednesday { get; set; }
        public string? RecurringThursday { get; set; }
        public string? RecurringFriday { get; set; }
        public string? RecurringSaturday { get; set; }
        public string? RecurringSunday { get; set; }
        public string? IsClearedToExit { get; set; }
        public DateTime ValidityStartDate { get; set; }
        public DateTime ValidityEndDate { get; set; }
        public DateTime AccessStartTime { get; set; }
        public DateTime AccessEndTime { get; set; }
        public string? isActiveBooking { get; set; }
        public DateTime BookingDate { get; set; }
        public string? QrCodeImage { get; set; }
    }
}
