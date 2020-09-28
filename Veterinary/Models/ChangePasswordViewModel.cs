using System.ComponentModel.DataAnnotations;

namespace Veterinary.Models
{
    public class ChangePasswordViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }



        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string Confirm { get; set; }

    }
}
