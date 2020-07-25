using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Models
{
    public class RegisterNewUserViewModel
    {
       
        public string FisrtName { get; set; }


        public string LastName { get; set; }


        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Address { get; set; }

        public string ZipCode { get; set; }


        public string TaxNumber { get; set; }


        public string Gender { get; set; }        


        public string PhoneNumber { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required]        
        public DateTime? DateOfBirth { get; set; }


        public string Nationality { get; set; }


        [Display(Name = "Document")]
        [Range(1, int.MaxValue, ErrorMessage = "You  must select a {0}")]
        public int DocumentTypeID { get; set; }

        public IEnumerable<SelectListItem> Documents { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }

    }
}
