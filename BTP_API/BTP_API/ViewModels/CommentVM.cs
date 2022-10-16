using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class CommentVM
    {
        [Required]
        public string Content { get; set; }
    }
}
