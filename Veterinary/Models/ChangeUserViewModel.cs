using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class ChangeUserViewModel
    {

        public string FirstName { get; set; }


        public string LastName { get; set; }


        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Address { get; set; }


        public string ZipCode { get; set; }


        public string TaxNumber { get; set; }


        public string Gender { get; set; }


        public string PhoneNumber { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }


        public string Nationality { get; set; }


        [Display(Name = "Document")]
        [Range(1, int.MaxValue, ErrorMessage = "You  must select a {0}")]
        public int DocumentTypeID { get; set; }

        public IEnumerable<DocumentType> Documents { get; set; }


        [Display(Name = "Nº Document")]
        [Required]
        [MaxLength(20, ErrorMessage = "The  field {0} only can contain {1} characters.")]
        [MinLength(5, ErrorMessage = "The  field {0} can contain minimum {1} characters.")]
        public string Document { get; set; }

    }
}
