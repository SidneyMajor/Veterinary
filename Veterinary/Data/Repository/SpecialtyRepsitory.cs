using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class SpecialtyRepsitory : GenericRepository<Specialty>, ISpecialtyRepository
    {
        private readonly DataContext _context;

        public SpecialtyRepsitory(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialty>> GetComboSpecialties()
        {

            return await _context.Specialties.Where(d => d.WasDeleted == false).ToListAsync();

        }
    }
}
