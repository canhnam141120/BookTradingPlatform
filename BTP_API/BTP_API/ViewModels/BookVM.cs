using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class BookVM
    {
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Author { get; set; }
        
        [Required]
        public string Publisher { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public string Language { get; set; }
        
        [Required]
        public int NumberOfPages { get; set; }
        
        [Required]
        public float Weight { get; set; }
        
        [Required]
        public float CoverPrice { get; set; }
        
        [Required]
        public float DepositPrice { get; set; }
        
        [Required]
        public string StatusBook { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        
        [Required]
        public bool IsExchange { get; set; }
        
        [Required]
        public bool IsRent { get; set; }
        
        [Required]
        public float RentFee { get; set; }
        
        [Required]
        public int NumberOfDays { get; set; }
    }
}
