using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IAppointmentRepsitory : IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetAllAppointmentlAsync(string username);

        Task<IQueryable<Appointment>> GetUserAppointmentDetailAsync(int animalid, string username);

        Task<Appointment> GetAppointmentByIdAsync(int id);

        Task<bool> CheckAppointmentAsync(Appointment model);

        Task<IQueryable<Appointment>> DoctorAppointmentsAsync(string username);

        Task<bool> CheckAppointmentAnimalIdAsync(int id);

        Task<bool> CheckAppointmentDoctorIdAsync(int id);

        Task<bool> CheckAppointmentUserdAsync(User user);
    }
}
