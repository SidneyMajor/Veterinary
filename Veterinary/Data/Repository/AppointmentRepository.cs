
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public AppointmentRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        /// <summary>
        /// Checks the customer's appearance and changes the status of the appointment
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Checks availability
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true/false</returns>
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

        /// <summary>
        /// Get all appointment by user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IQueryable<Appointment></returns>
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

        /// <summary>
        /// Get appointment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Appointment</returns>
        public async Task<Appointment> GetAppointmentByIdAsync(int id)
        {
            return await _context.Appointments
                 .Include(a => a.Animal)
                 .Include(a => a.Doctor)
                 .Include(a=> a.Specialty)
                 .FirstOrDefaultAsync(m => m.Id == id && m.WasDeleted == false);
        }


        /// <summary>
        /// Get appointment by animal id and username
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns>Appointment</returns>
        public async Task<IQueryable<Appointment>> GetUserAppointmentDetailAsync(int animalid, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            return  _context.Appointments.Where(a => a.User == user && a.AnimalID == animalid && a.WasDeleted == false)
                .Include(a=> a.Doctor)
                .Include(a=> a.Specialty);
        }

        /// <summary>
        /// Doctor appointment
        /// </summary>
        /// <param name="username"></param>
        /// <returns>IQueryable<Appointment></returns>
        public async Task<IQueryable<Appointment>> DoctorAppointmentsAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.User == user);

            return _context.Appointments.Include(a => a.Animal).Include(a => a.Specialty)
               .Include(a => a.User).Include(a => a.Doctor).Where(a => a.WasDeleted == false && a.Doctor.Equals(doctor) && a.StartTime.Date == DateTime.Today.Date ).OrderByDescending(a => a.StartTime);
        }
    }
}
