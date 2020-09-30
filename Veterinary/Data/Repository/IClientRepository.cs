using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client> GetClientByUserEmailAsync(string email);

        IEnumerable<Client> ClientsDelete();

    }
}
