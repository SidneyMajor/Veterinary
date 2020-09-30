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

        public IQueryable<Doctor> GetDoctorsSpecialtyId(int specialtyId)
        {
            return _context.Doctors.Where(d => d.SpecialtyID == specialtyId);
        }

        public async Task<Doctor> GetDoctorByUserEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(c => c.User.Email == email);
        }

        public async Task<IEnumerable<Doctor>> GetComboDoctors()
        {

            return await _context.Doctors.Where(d => d.WasDeleted == false).ToListAsync();

        }
    }
}
