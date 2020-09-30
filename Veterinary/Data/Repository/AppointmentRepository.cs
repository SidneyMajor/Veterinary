
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Helpers;

namespace Veterinary.Data.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepsitory
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        //DateTime.Now.Hour >= 18 || DateTime.Now.TimeOfDay > Convert.ToDateTime(lblHora.Text).TimeOfDay
        public AppointmentRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        private async Task NoShowAppointment()
        {
            await _context.Appointments.ForEachAsync(a =>
           {

               if (a.StartTime.Date < DateTime.Now.Date && a.Status == "Accepted")
               {
                   a.Status = "No-show";
               }
               _context.Update(a);

           });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckAppointmentAsync(Appointment model)
        {
            if (model.StartTime < DateTime.Now)
            {
                return true;
            }

            if ((model.StartTime.Hour>=18 && model.StartTime.Hour<=24) || (model.StartTime.Hour<8 && model.StartTime.Hour>=0))
            {
                return true;
            }
            
            return await _context.Appointments.AnyAsync(a => a.StartTime.Equals(model.StartTime) &&
            a.EndTime.Equals(model.EndTime) && a.DoctorID.Equals(model.DoctorID) && (a.Status.Equals("Accepted") || a.Status.Equals("Pending")));
        }


        public async Task<IQueryable<Appointment>> GetAllAppointmentlAsync(string username)
        {
            await NoShowAppointment();
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

            if (await _userHelper.IsUserInRoleAsync(user, "Doctor"))
            {
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.User == user);

                return _context.Appointments.Include(a => a.Animal).Include(a => a.Specialty)
                   .Include(a => a.User).Include(a => a.Doctor).Where(a => a.WasDeleted == false && a.Doctor.Equals(doctor)).OrderByDescending(a => a.StartTime);
            }

            return _context.Appointments.Include(a => a.Animal).Include(a => a.Doctor).Include(a => a.Specialty)
                .Where(a => a.User == user && a.WasDeleted == false)
                .OrderByDescending(a => a.StartTime);
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
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


        public async Task<IQueryable<Appointment>> DoctorAppointmentsAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.User == user);

            return _context.Appointments.Include(a => a.Animal).Include(a => a.Specialty)
               .Include(a => a.User).Include(a => a.Doctor).Where(a => a.WasDeleted == false && a.Doctor.Equals(doctor) && a.StartTime.Date == DateTime.Today.Date ).OrderByDescending(a => a.StartTime);
        }
    }
}
