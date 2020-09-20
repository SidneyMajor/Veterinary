using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class Animal : IEntity
    {
        public int Id { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public bool WasDeleted { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string Name { get; set; }

        [Required]
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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "You  must select a Species.")]
        public int SpeciesID { get; set; }


        public Species Species { get; set; }


        public User User { get; set; }


        public string ImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ImageUrl))
                {
                    return this.ImageUrl.Replace("~", "..");
                }
                else
                {
                    return "../images/img.jpg";
                }


            }
        }
    }

}
