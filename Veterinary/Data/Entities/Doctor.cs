using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class Doctor:Person
    {

        [MinLength(9, ErrorMessage = "The  field {0} can contain minimum {1} characters.")]
        public string SSNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You  must select a Specialty.")]
        public int SpecialtyID { get; set; }

       
        public Specialty Specialty { get; set; }


        //public int ClinicID { get; set; }


        //public Clinic Clinic { get; set; }



        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
