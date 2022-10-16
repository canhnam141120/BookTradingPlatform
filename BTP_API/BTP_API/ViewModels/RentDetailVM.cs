using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class RentDetailVM
    {
        public string BeforeStatusBook { get; set; }
        
        public string AfterStatusBook { get; set; }
        
        [Required]
        public string StorageStatusBook { get; set; }
        
        [Required]
        public string Status { get; set; }
    }
}
