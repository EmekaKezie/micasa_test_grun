namespace TheEstate.Models.AppModels
{
    public class TransactionModelResponse
    {
        public string? AccessCode { get; set; }
        public string? Reference { get; set; }
        public string? SecretKey { get; set; }
        public string? PublicKey { get; set; }
        public string? AuthourizedUrl { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
    }
}
