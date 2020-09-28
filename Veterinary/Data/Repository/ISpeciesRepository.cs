using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface ISpeciesRepository : IGenericRepository<Species>
    {
        Task<IEnumerable<Species>> GetComboSpecies();
    }
}
