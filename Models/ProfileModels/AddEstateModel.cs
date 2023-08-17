namespace TheEstate.Models.ProfileModels
{
    public class AddEstateModel
    {
        public string ResidentCode { get; set; } = string.Empty;
        public string ProfileId { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
