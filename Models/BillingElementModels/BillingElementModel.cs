namespace TheEstate.Models.BillingElementModels
{
    public class BillingElementModel
    {
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        public string? ElementTitle { get; set; }  
        public string? ElementStatus { get; set; }
        public string? HouseholdClassification { get; set; }
        public decimal ElementCost { get; set; }
    }
}
