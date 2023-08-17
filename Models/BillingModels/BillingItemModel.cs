namespace TheEstate.Models.BillingModels
{
    public class BillingItemModel
    {
        public string? BillId { get; set; }
        public string? ItemId { get; set; }
        public string? ItemDesc { get; set; }
        public decimal ItemCost { get; set; }
        public string? ItemCurrency { get; set; }

    }
}
