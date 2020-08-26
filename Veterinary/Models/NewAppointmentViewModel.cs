﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class NewAppointmentViewModel:Appointment
    {
        public IEnumerable<Animal> Animals { get; set; }

        public IEnumerable<Doctor> Doctors { get; set; }
    }
}
