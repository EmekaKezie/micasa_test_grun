namespace TheEstate.Models.StreetModels
{
    public class StreetModelView : StreetModel
    {
        public string? StreetId { get; set; }
        public string? ZipcodeId { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneType { get; set; }
        public string? EstateName { get; set; }
        public string? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifedDate { get; set; }
    }
}
