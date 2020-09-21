using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IAppointmentRepsitory:IGenericRepository<Appointment>
    {
        Task<IQueryable<Appointment>> GetAllAppointmentlAsync(string username);

        Task<Appointment> GetUserAppointmentDetailAsync(int id, string username);

        Task<Appointment> GetAppointmentByIdAsync(int id);

        Task<bool> CheckAppointmentAsync(Appointment model);

        Task<IQueryable<Appointment>> DoctorAppointmentsAsync(string username);
    }
}
