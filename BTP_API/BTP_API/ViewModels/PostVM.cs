using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class PostVM
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public string Hashtag { get; set; } 
    }
}
