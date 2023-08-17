using TheEstate.Models.BillingModels;

namespace TheEstate.Models.PaymentModels
{
    public class PaymentModelOffline : PaymentModelCreate
    {
        public decimal Amount { get; set; }
        public string? Receipt { get; set; }
        public string? Note { get; set; }
    }
}
