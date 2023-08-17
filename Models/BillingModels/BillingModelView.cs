namespace TheEstate.Models.BillingModels
{
    public class BillingModelView: BillingModel
    {
        public DateTime InvoiceDate { get; set; }
        public string? EstateName { get; set; }
        public string? EstateDesc { get; set; }
        public string? BillId { get; set; }
        public string? BillStatus { get; set; }
        public DateTime BillStatusDate { get; set; }
        public decimal BillAmount { get; set; }
        public string? HouseholdLabel { get; set; }
        public string? HouseholdUsageCategory { get; set; }
        public string? PropertyId { get; set; }
        public string? ResidentMobileNo { get; set; }
        public string? ResidentEmail { get; set; }
        public string? InvoiceTitle { get; set; }
        public string? InvoiceStatus { get; set; }
        public string? InvoiceTargetFlag { get; set; }
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentAcceptanceStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public string? PaymentReceipt { get; set; }
        public string? PaymentNote { get; set; }
        public List<BillingItemModel>? BillItems { get; set; }
    }
}
