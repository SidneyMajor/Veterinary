using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface ISpecialtyRepository : IGenericRepository<Specialty>
    {
        Task<IEnumerable<Specialty>> GetComboSpecialties();
    }
}
