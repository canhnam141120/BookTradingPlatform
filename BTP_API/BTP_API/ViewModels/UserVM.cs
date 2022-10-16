using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class UserVM
    {
        [Required]
        public string Fullname { get; set; }

        public int? Age { get; set; }

        [Required]
        public string AddressMain { get; set; }

        public string AddressSub1 { get; set; }

        public string AddressSub2 { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
