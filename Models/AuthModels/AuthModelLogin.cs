using System.ComponentModel.DataAnnotations;

namespace TheEstate.Models.AuthModels
{
    public class AuthModelLogin
    {
        [Required]
        public string? Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
