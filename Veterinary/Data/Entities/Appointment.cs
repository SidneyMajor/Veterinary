using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public bool WasDeleted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime AppointmentTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }


        public string Remarks { get; set; }


        public string Status { get; set; }

        
        public User User { get; set; }


        public int AnimalID { get; set; }
        
        
        public Animal Animal { get; set; }


        public int DoctorID { get; set; }


        public Doctor Doctor { get; set; }
    }
}
