namespace TheEstate.Models.InvoiceModels
{
    public class InvoiceItemModel
    {
        //public string? InvoiceId { get; set; }
        public string? InvoiceItemTitle { get; set; }
        public string? InvoiceCostType { get; set; }
        public decimal InvoiceFlatRateCost { get; set; }
        public string? BillingElementId { get; set; }
    }
}
