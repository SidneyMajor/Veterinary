using System.Collections.Generic;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AdminAnimalDetailViewModel
    {
        public Client Client { get; set; }

        public Animal Animal { get; set; }

        public IEnumerable<Appointment> GetAppointments { get; set; }
    }
}
