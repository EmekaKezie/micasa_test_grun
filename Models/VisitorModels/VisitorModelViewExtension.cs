namespace TheEstate.Models.VisitorModels
{
    public class VisitorModelViewExtension : VisitorModel
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
        public List<string>? RecurringDays { get; set; }
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
