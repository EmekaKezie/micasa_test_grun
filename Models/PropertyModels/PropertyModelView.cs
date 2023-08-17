namespace TheEstate.Models.PropertyModels
{
    public class PropertyModelView : PropertyModel
    {
        public string? PropertyId { get; set; }
        public string? EstateName { get; set; }
        public string? ZoneId { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneType { get; set; }
        public string? StreetId { get; set; }
        public string? StreetCode { get; set; }
        public string? StreetName { get; set; }
        public string? StreetType { get; set; }
        public string? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifedDate { get; set; }
    }
}
