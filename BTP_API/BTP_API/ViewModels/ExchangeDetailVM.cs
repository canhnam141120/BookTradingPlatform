using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class ExchangeDetailVM
    {
        public string BeforeStatusBook1 { get; set; }
        
        public string AfterStatusBook1 { get; set; }
        
        [Required]
        public string StorageStatusBook1 { get; set; }
        
        public string BeforeStatusBook2 { get; set; }
        
        public string AfterStatusBook2 { get; set; }
        
        [Required]
        public string StorageStatusBook2 { get; set; }
        
        [Required]
        public string Status { get; set; }
    }
}
