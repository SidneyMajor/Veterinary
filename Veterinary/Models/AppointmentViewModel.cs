using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AppointmentViewModel : Appointment
    {
        public IEnumerable<Animal> Animals { get; set; }

        public IEnumerable<Specialty> Specialties { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }

        public IEnumerable<Appointment> GetAppointments { get; set; }
    }
}
