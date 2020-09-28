using Veterinary.Data.Entities;

namespace Veterinary.Models
{
    public class DoctorAppointmentViewModel : Appointment
    {
        public Client GetClient { get; set; }
    }
}
