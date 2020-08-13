using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class SpeciesRepository:GenericRepository<Species>, ISpeciesRepository
    {
        public SpeciesRepository(DataContext context):base(context)
        {

        }
    }
}
