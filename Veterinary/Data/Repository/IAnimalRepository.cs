using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
   public interface IAnimalRepository:IGenericRepository<Animal>
    {
        //get a animal
        Task<IQueryable<Animal>> GetAllAnimalAsync(string username);

        //get a animal
        Task<Animal> GetDetailAnimalAsync(int id, string username);
    }
}
