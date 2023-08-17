using System.ComponentModel.DataAnnotations;

namespace TheEstate.Models.InvoiceModels
{
    public class InvoiceModel
    {
        [Required]
        public string? EstateId { get; set; }
        public string? ZoneId { get; set; }
        [Required] 
        public string? InvoiceTitle { get; set; }
        [Required]
        public string? InvoiceStatus { get; set; }
        [Required]
        public string? InvoiceTargetFlag { get; set; }
        
    }
}
