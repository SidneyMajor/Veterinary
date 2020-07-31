using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        //public DbSet<Animal> Animals { get; set; }
        //public DbSet<Doctor> Doctors { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {     
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            // Cascading Delete Rule
            var cascadeFKs = modelbuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }
    }

}
