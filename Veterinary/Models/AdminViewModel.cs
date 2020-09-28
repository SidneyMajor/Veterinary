using System.Collections.Generic;
using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class AdminViewModel
    {
        public IEnumerable<DocumentType> GetDocumentTypes { get; set; }

        public IEnumerable<Species> GetSpecies { get; set; }

        public IEnumerable<Specialty> GetSpecialties { get; set; }

        public IEnumerable<Appointment> GetAppointments { get; set; }

        public int NClients { get; set; }

        public int NDoctors { get; set; }

        public int NAnimals { get; set; }

        public int NAppointments { get; set; }
    }
}
