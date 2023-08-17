namespace TheEstate.Models.AppModels
{
    public class SmtpModel
    {
        public string? Host { get; set; }
        public string? Sender { get; set; }
        public string? Username { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}
