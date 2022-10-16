using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ForgotPasswordCode { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
