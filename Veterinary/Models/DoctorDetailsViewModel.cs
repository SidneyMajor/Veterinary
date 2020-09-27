using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class DoctorDetailsViewModel
    {
        public Doctor GetDoctor { get; set; }
        public IEnumerable<Appointment> GetAppointments { get; set; }
    }
}
