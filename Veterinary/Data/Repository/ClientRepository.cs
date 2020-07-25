using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class ClientRepository:GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {

        }
    }
}
