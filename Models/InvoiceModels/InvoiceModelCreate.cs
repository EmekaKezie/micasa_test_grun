namespace TheEstate.Models.InvoiceModels
{
    public class InvoiceModelCreate : InvoiceModel
    {
        public List<string>? targetHouseholds { get; set; }
        public string? InvoiceItemBase64 { get; set; }
        public string? ProfileId { get; set; }
        //public List<InvoiceItemModel> InvoiceItem { get; set; }
    }
}
