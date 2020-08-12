using System.ComponentModel.DataAnnotations;

namespace Veterinary.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
