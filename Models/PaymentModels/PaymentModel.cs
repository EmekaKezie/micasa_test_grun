namespace TheEstate.Models.PaymentModels
{
    public class PaymentModel
    {
        public string? EstateId { get; set; }
        public string? HouseholdId { get; set; }
        public string? BillId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentGateway { get; set; }
        public string? AcceptanceStatus { get; set; }

    }
}
