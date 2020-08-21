using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class SpecialtyRepsitory : GenericRepository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepsitory(DataContext context) : base(context)
        {

        }
    }
}
