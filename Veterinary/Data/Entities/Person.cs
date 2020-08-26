using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public abstract class Person:IEntity
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Address { get; set; }


        public string ZipCode { get; set; }


        public string City { get; set; }


        public string TaxNumber { get; set; }


        public string Gender { get; set; }


        public string PhoneNumber { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        public string Nationality { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }


        public bool WasDeleted { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public User User { get; set; }


        [Display(Name = "Nº Document")]
        [Required]
        [MaxLength(20, ErrorMessage = "The  field {0} only can contain {1} characters.")]
        [MinLength(5, ErrorMessage = "The  field {0} can contain minimum {1} characters.")]
        public string Document { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You  must select a Document Type.")]
        public int DocumentTypeID { get; set; }


        public DocumentType DocumentType { get; set; }

        
    }
}
