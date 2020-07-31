using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class ClientRepository:GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Client> GetClientByUserEmailAsync(string email)
        {
            return await _context.Clients.Where(c => c.User.Email == email).FirstOrDefaultAsync();
        }
    }
}
