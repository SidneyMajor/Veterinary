using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly DataContext _context;

        public DoctorRepository(DataContext context) : base(context)
        {
            _context = context;
        }



        /// <summary>
        /// Get doctor by specialty id
        /// </summary>
        /// <param name="specialtyId"></param>
        /// <returns>doctor</returns>
        public IQueryable<Doctor> GetDoctorsSpecialtyId(int specialtyId)
        {
            return _context.Doctors.Where(d => d.SpecialtyID == specialtyId);
        }


        /// <summary>
        /// Get doctor by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>doctor</returns>
        public async Task<Doctor> GetDoctorByUserEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(c => c.User.Email == email);
        }


        /// <summary>
        /// Get active doctors 
        /// </summary>
        /// <returns>list doctors</returns>
        public async Task<IEnumerable<Doctor>> GetComboDoctors()
        {

            return await _context.Doctors.Where(d => d.WasDeleted == false).ToListAsync();

        }

        public IEnumerable<Doctor> DoctorsDelete()
        {
            return _context.Doctors.Where(d => d.WasDeleted == true).Include(d=> d.User).ToList();
        }
    }
}
