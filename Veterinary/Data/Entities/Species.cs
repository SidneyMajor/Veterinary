using System;
using System.ComponentModel.DataAnnotations;

namespace Veterinary.Data.Entities
{
    public class Species:IEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Is Inactive?")]
        public bool WasDeleted { get; set; }

        [Required]
        public string Description { get; set; }
    }
}