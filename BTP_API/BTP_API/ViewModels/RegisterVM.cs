using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Fullname { get; set; } 

        [Required]
        public string Phone { get; set; } 

        [Required]
        public string AddressMain { get; set; }
    }
}
