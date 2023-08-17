namespace TheEstate.Models.HouseholdModels
{
    public class HouseholdModelView : HouseholdModel
    {
        public string? HouseholdId { get; set; }
        public string? ZipcodeId { get; set; }
        public string? StreetId { get; set; }
        public string? EstateName { get; set; }
        public string? StreetCode { get; set; }
        public string? StreetName { get; set; }
        public string? StreetType { get; set; }
        public string? ZoneId { get; set; }
        public string? ZoneCode { get; set; }
        public string? ZoneName { get; set; }
        public string? ZoneType { get; set; }
        public string? PropertyNo { get; set; }
        public string? PropertyType { get; set; }
        public string? PropertyCategory { get; set; }
        public string? PropertyOwnerFirstname { get; set; }
        public string? PropertyOwnerLastname { get; set; }
        public string? PropertyOwnerMobileNo { get; set; }
        public string? PropertyOwnerEmail { get; set; }
        public string? PropertyOwnerGender { get; set; }
        public string? PropertyOwnerOtherAddress { get; set; }
        //public string? PrimaryResidentCode { get; set; }
        //public string? PrimaryResidentCategory { get; set; }
        //public string? PrimaryResidentFirstname { get; set; }
        //public string? PrimaryResidentLastname { get; set; }
        //public string? PrimaryResidentMobileNo { get; set; }
        //public string? PrimaryResidentEmail { get; set; }
        //public string? PrimaryResidentGender { get; set; }
        //public string? PrimaryResidentStatus { get; set; }
        public string? EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifedDate { get; set; }
    }
}
