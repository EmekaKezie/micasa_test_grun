namespace TheEstate.Models.InvoiceModels
{
    public class InvoiceModelView:InvoiceModel
    {
        public string? InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? EstateName { get; set; }
        public string? EstateDesc { get; set; }
        public string? EstateCode { get; set; }
        public string? EstateImage { get; set; }
        public string? ZoneName { get; set; }
        public List<InvoiceItemModelView>? InvoiceItems { get; set; }
    }
}
