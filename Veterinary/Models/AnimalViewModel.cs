using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AnimalViewModel : Animal
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<Species> GetSpecies { get; set; }

        public DateTime? SelectDate { get; set; }
    }
}
