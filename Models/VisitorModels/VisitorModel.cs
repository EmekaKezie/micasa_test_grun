namespace TheEstate.Models.VisitorModels
{
    public class VisitorModel
    {
        public string? BookingTitle { get; set; }
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        public string? ResidentId { get; set; }
        public string? VisitorFullname { get; set; }
        public string? VisitorMobileNo { get; set; }
        public string? VisitorEmail { get; set; }
        public string? IsAccompanied { get; set; }
        public string? VisitorCompanions { get; set; }
        public string? PassageMode { get; set; } //SINGLE, MULTIPLE
        public string? IsRecurring { get; set; }
        public string? IsExitClearanceRequired { get; set; }
        public string? QrCodeText { get; set; }
        
    }
}
