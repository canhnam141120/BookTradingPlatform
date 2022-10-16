using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class FeedbackVM
    {
        [Required]
        public string Content { get; set; }
    }
}
