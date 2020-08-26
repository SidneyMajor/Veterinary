
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

        public AppointmentRepository(DataContext context, IUserHelper userHelper): base(context)
        {
           _context = context;
           _userHelper = userHelper;
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
                return _context.Appointments.Include(a => a.Animal).Include(a => a.Doctor)
                    .ThenInclude(a => a.User).OrderByDescending(a => a.AppointmentDate);
            }

            return _context.Appointments.Include(a => a.Animal).Include(a => a.Doctor)
                .Where(a => a.User == user && a.WasDeleted == false)
                .OrderByDescending(a => a.AppointmentDate);
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
