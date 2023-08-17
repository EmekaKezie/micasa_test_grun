namespace TheEstate.Models.EstateModels
{
    public class EstateModelView:EstateModel
    {
        public string? EstateId { get; set; }
        public string? EstateImage { get; set; }
        public int NoOfZone { get; set; }
        public int NoOfStreet { get; set; }
        public string? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifedDate { get; set; }
    }
}
