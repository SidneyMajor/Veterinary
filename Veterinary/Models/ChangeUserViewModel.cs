using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class ChangeUserViewModel:Person
    {

        public IEnumerable<DocumentType> Documents { get; set; }



        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SelectDate { get; set; }



        [Display(Name = "Image")]        
        public IFormFile ImageFile { get; set; }


        public string SSNumber { get; set; }


        public int SpecialtyID { get; set; }


        public IEnumerable<Specialty> Specialties { get; set; }

    }
}
