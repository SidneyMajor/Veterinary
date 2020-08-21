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


        public string SSNumber { get; set; }


        public int SpecialtyID { get; set; }


        public Specialty Specialty { get; set; }


        public int ClinicID { get; set; }


        public Clinic Clinic { get; set; }



        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}
