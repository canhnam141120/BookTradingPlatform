using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
