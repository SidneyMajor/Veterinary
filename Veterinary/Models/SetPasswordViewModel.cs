using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Models
{
    public class SetPasswordViewModel
    {

        public string UserId { get; set; }

       
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }



        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string Confirm { get; set; }


        public string FullName { get; set; }
    }
}
