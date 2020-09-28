using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AppointmentViewModel : Appointment
    {
        public IEnumerable<SelectListItem> Animals { get; set; }

        public IEnumerable<SelectListItem> Specialties { get; set; }

        public IEnumerable<SelectListItem> Doctors { get; set; }

        public IEnumerable<Appointment> GetAppointments { get; set; }
    }
}
