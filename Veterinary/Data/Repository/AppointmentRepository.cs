
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Helpers;

namespace Veterinary.Data.Repository
{
    public class AppointmentRepository:GenericRepository<Appointment>, IAppointmentRepsitory
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        //DateTime.Now.Hour >= 18 || DateTime.Now.TimeOfDay > Convert.ToDateTime(lblHora.Text).TimeOfDay
        public AppointmentRepository(DataContext context, IUserHelper userHelper): base(context)
        {
           _context = context;
           _userHelper = userHelper;
        }

        public async Task<bool> CheckAppointmentAsync(Appointment model)
        {
            if (model.StartTime < DateTime.Now || model.StartTime.Hour>=18 )
            {
                return true;
            }

            return await _context.Appointments.AnyAsync(a => a.StartTime.Equals(model.StartTime) &&
            a.EndTime.Equals(model.EndTime) &&  a.DoctorID.Equals(model.DoctorID));
        }

        public async Task<IQueryable<Appointment>> GetAllAppointmentlAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Appointments.Include(a => a.Animal).Include(a => a.Doctor).Include(a => a.Specialty)
                    .Include(a => a.User).Where(a => a.WasDeleted == false).OrderByDescending(a => a.StartTime);
            }
           
            return  _context.Appointments.Include(a => a.Animal).Include(a => a.Doctor).Include(a => a.Specialty)
                .Where(a => a.User == user && a.WasDeleted == false)
                .OrderByDescending(a => a.StartTime);
        }

        public async  Task<Appointment> GetAppointmentByIdAsync(int id)
        {
           return await _context.Appointments
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id && m.WasDeleted == false);
        }

        public async Task<Appointment> GetUserAppointmentDetailAsync(int id, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            return await _context.Appointments.FirstOrDefaultAsync(a => a.User == user && a.Id == id && a.WasDeleted == false);
        }
    }
}
