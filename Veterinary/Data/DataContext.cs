using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Veterinary.Data.Entities;

namespace Veterinary.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Species> Species { get; set; }

        public DbSet<Animal> Animals { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        //public DbSet<Clinic> Clinics { get; set; }


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
            //Unique Index
            modelbuilder.Entity<Doctor>().HasIndex(d => d.SSNumber).IsUnique();
            modelbuilder.Entity<Doctor>().HasIndex(d => d.TaxNumber).IsUnique();
            modelbuilder.Entity<Doctor>().HasIndex(d => d.Document).IsUnique();
            modelbuilder.Entity<Client>().HasIndex(c => c.TaxNumber).IsUnique();
            modelbuilder.Entity<Client>().HasIndex(c => c.Document).IsUnique();

            modelbuilder.Entity<DocumentType>().HasIndex(c => c.Description).IsUnique();
            modelbuilder.Entity<Specialty>().HasIndex(c => c.Description).IsUnique();
            modelbuilder.Entity<Species>().HasIndex(c => c.Description).IsUnique();


            base.OnModelCreating(modelbuilder);
        }

    }

}
