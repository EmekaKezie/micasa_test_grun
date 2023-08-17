namespace TheEstate.Models.ZoneModels
{
    public class ZoneModelView:ZoneModel
    {
        public string? ZoneId { get; set; }
        public string? EstateName { get; set; }
        public string? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifedDate { get; set; }
    }
}
