using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class FeeVM
    {
        [Required]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public float Price { get; set; }
    }
}
