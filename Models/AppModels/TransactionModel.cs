namespace TheEstate.Models.AppModels
{
    public class TransactionModel
    {
        public string? EstateId { get; set; }
        public string? HouseholdId { get; set; }
        public string? BillId { get; set; }
        public Decimal Amount { get; set; }
        public string? Email { get; set; }
        
        public string? Currency { get; set; }
        public string? Description { get; set; }
    }
}
