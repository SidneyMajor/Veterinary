using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Client> ClientsDelete()
        {
            return  _context.Clients.Where(c => c.WasDeleted == true).Include(c=>c.User).ToList();
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
