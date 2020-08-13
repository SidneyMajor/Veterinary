using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class RegisterNewUserViewModel:Client
    {
        public IEnumerable<DocumentType> Documents { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SelectDate { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }


        [Display(Name = "Image")]        
        public IFormFile ImageFile { get; set; }

    }
}
