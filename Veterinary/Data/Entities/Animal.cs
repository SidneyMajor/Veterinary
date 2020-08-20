using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class Animal:IEntity
    {
        public int Id { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public bool WasDeleted { get; set ; }


        public string Name { get; set; }


        public string Breed { get; set; }  
        

        public string Gender { get; set; }


        public string Color { get; set; }


        public double Weight { get; set; }


        public double Size { get; set; }


        public string Remarks { get; set; }


        public string ImageUrl { get; set; }


        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        public int SpeciesID { get; set;}


        public Species Species { get; set; }


        public User User { get; set; }

    }
}
