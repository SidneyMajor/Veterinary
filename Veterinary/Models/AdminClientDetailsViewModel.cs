using System.Collections.Generic;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AdminClientDetailsViewModel
    {
        public Client GetClient { get; set; }

        public IEnumerable<Animal> GetAnimals { get; set; }
    }
}
