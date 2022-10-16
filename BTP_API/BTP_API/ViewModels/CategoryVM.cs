using System.ComponentModel.DataAnnotations;

namespace BTP_API.ViewModels
{
    public class CategoryVM
    {
        [Required]
        public string Name { get; set; }
    }
}
