using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class RegisterNewDoctorViewModel : Doctor
    {
        public IEnumerable<DocumentType> Documents { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SelectDate { get; set; }

        //[Required]
        //public string Password { get; set; }

        //[Required]
        //[Compare("Password")]
        //public string Confirm { get; set; }


        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }



        public IEnumerable<Specialty> Specialties { get; set; }
    }
}
