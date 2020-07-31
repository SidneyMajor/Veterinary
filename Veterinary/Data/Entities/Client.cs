﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }


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
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        public string Nationality { get; set; }


        public string FullName { get { return $"{FirstName} {LastName}"; } }


        public bool WasDeleted { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public User User { get; set; }


        [Display(Name = "Nº Document")]
        [Required(ErrorMessage = "The field {0} only can contain {1} characters.")]
        [StringLength(20, ErrorMessage = "The  field {0} must contain between {2} and {1} characters", MinimumLength = 5)]
        public string Document { get; set; }


        public int DocumentTypeID { get; set; }



        public DocumentType DocumentType { get; set; }
    }
}
