using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data
{
    public class DataContext:DbContext
    {
        //public DbSet<Client> Clients { get; set; }
        //public DbSet<Animal> Animals { get; set; }
        //public DbSet<Employee> Employees { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
