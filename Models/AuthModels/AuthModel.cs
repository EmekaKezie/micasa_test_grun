using TheEstate.Models.ResidentModels;

namespace TheEstate.Models.AuthModels
{
    public class AuthModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? LoginMethod { get; set; }
        public string? MobileNo { get; set; }
        public string? ImageUrl { get; set; }
        public string? OnboadingStage { get; set; }
        public List<ResidentModelView>? Residency { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
