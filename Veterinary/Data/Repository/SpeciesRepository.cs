using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class SpeciesRepository : GenericRepository<Species>, ISpeciesRepository
    {
        private readonly DataContext _context;

        public SpeciesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Species>> GetComboSpecies()
        {

            return await _context.Species.Where(d => d.WasDeleted == false).ToListAsync();

        }
    }
}
