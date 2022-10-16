using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class RentBillVM
    {
        public DateOnly? SendDate { get; set; }
        
        public DateOnly? ReceiveDate { get; set; }
        
        public DateOnly? RecallDate { get; set; }
        
        public DateOnly? RefundDate { get; set; }
        
        [Required]
        public bool IsPaid { get; set; }
        
        public DateTime? PaidDate { get; set; }
        
        public string Payment { get; set; }
    }
}
