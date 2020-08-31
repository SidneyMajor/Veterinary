using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IDoctorRepository: IGenericRepository<Doctor>
    {
        Task<Doctor> GetDoctorByUserEmailAsync(string email);

        //IQueryable<Doctor> GetDoctorBySpecialtyAsync(int id);
    }
}
