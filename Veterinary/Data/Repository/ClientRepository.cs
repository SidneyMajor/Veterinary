using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        /// <summary>
        /// Get client by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>client</returns>
        public async Task<Client> GetClientByUserEmailAsync(string email)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.User.Email == email);
        }
    }
}
