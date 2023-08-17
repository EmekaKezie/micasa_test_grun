namespace TheEstate.Models.PaymentModels
{
    public class PaymentModelCreate
    {
        public string? EstateId { get; set; }
        public string? HouseholdId { get; set; }
        public string? BillId { get; set; }
        public string? ReferenceCode { get; set; }
        public string? AccessCode { get; set; }
        public string? PaymentGateway { get; set; }
        //public int PaymentAmount { get; set; }
        //public string? PaymentDesc { get; set; }
        //public string? PaymentMethod { get; set; }
    }
}
