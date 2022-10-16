using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class ChangePasswordVM
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
