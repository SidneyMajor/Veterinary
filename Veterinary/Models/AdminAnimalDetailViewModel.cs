using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AdminAnimalDetailViewModel
    {
        public Client Client { get; set; }

        public Animal Animal { get; set; }
    }
}
