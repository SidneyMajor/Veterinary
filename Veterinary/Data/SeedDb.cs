using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Helpers;

namespace Veterinary.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {

            //Certificar a base de dados esta criada se não ela a cria.
            await _context.Database.EnsureCreatedAsync();
            //Checke Role
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Doctor");
            //Add document types
            if (!_context.DocumentTypes.Any())
            {
                var document = new DocumentType
                {
                    Description = "Cartão do Cidadão",
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                };

                _context.DocumentTypes.Add(document);
                await _context.SaveChangesAsync();
            }
            //add species
            if (!_context.Species.Any())
            {
                var species = new Species
                {
                    Description = "Cat",
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                };

                _context.Species.Add(species);
                await _context.SaveChangesAsync();
            }

            //add specialty
            if (!_context.Specialties.Any())
            {
                var specialty = new Specialty
                {
                    Description = "Consulta geral",
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                };

                _context.Specialties.Add(specialty);
                await _context.SaveChangesAsync();
            }

            //Add client/admin/animal
            if (!_context.Clients.Any())
            {

                var userAdmin = await _userHelper.GetUserByEmailAsync("Sidney.major@seed.pt");

                var userCustomer = await _userHelper.GetUserByEmailAsync("Isabel@seed.pt");

                if (userAdmin == null)
                {
                    userAdmin = new User
                    {
                        Email = "Sidney.major@seed.pt",
                        UserName = "Sidney.major@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(userAdmin, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                if (userCustomer == null)
                {
                    userCustomer = new User
                    {
                        Email = "Isabel@seed.pt",
                        UserName = "Isabel@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(userCustomer, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                var isInRoleAdmin = await _userHelper.IsUserInRoleAsync(userAdmin, "Admin");
                var isInRoleCustomer = await _userHelper.IsUserInRoleAsync(userCustomer, "Customer");

                if (!isInRoleAdmin)
                {
                    await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");
                }

                if (!isInRoleCustomer)
                {
                    await _userHelper.AddUserToRoleAsync(userCustomer, "Customer");
                }
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userAdmin);
                await _userHelper.ConfirmEmailAsync(userAdmin, token);

                var tokenCustomer = await _userHelper.GenerateEmailConfirmationTokenAsync(userCustomer);
                await _userHelper.ConfirmEmailAsync(userCustomer, tokenCustomer);

                this.AddClient("Sidney", "Major", userAdmin);
                this.AddClient("Isabel", "Frazão", userCustomer);

                this.AddAnimal("Piter", userCustomer);
                this.AddAnimal("Xana", userCustomer);

                await _context.SaveChangesAsync();

            }
            //Add Doctor
            if (!_context.Doctors.Any())
            {

                var userDoctor = await _userHelper.GetUserByEmailAsync("Doctor@seed.pt");

                if (userDoctor == null)
                {
                    userDoctor = new User
                    {
                        Email = "Doctor@seed.pt",
                        UserName = "Doctor@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(userDoctor, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }


                var isInRoleDoctor = await _userHelper.IsUserInRoleAsync(userDoctor, "Doctor");


                if (!isInRoleDoctor)
                {
                    await _userHelper.AddUserToRoleAsync(userDoctor, "Doctor");
                }


                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userDoctor);
                await _userHelper.ConfirmEmailAsync(userDoctor, token);

                this.AddDoctor("Tiago", "Silveira", userDoctor);

                await _context.SaveChangesAsync();

            }
        }

        private void AddClient(string name, string apelido, User user)
        {
            _context.Clients.Add(new Client
            {
                FirstName = name,
                LastName = apelido,
                Address = "Rua dos milagres",
                ZipCode = "0000-000",
                City = "Loures",
                DocumentTypeID = _context.DocumentTypes.FirstOrDefault().Id,
                DocumentType = _context.DocumentTypes.FirstOrDefault(),
                Document = _random.Next(10000, 999999).ToString(),
                TaxNumber = _random.Next(100000000, 399999999).ToString(),
                DateOfBirth = new DateTime(_random.Next(1930, 2020), _random.Next(1, 12), _random.Next(1, 32)),
                Gender = "N/N",
                User = user,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
            });
        }


        private void AddAnimal(string name, User user)
        {
            _context.Animals.Add(new Animal
            {
                Name = name,
                Breed = "Desconhecido",
                DateOfBirth = new DateTime(_random.Next(2000, 2020), _random.Next(1, 12), _random.Next(1, 32)),
                Gender = "F",
                SpeciesID = _context.Species.FirstOrDefault().Id,
                Species = _context.Species.FirstOrDefault(),
                User = user,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
            });
        }


        private void AddDoctor(string name, string apelido, User user)
        {
            _context.Doctors.Add(new Doctor
            {
                FirstName = name,
                LastName = apelido,
                Address = "Rua dos dotores",
                ZipCode = "0000-000",
                City = "Loures",
                DocumentTypeID = _context.DocumentTypes.FirstOrDefault().Id,
                DocumentType = _context.DocumentTypes.FirstOrDefault(),
                Document = _random.Next(10000, 999999).ToString(),
                TaxNumber = _random.Next(100000000, 399999999).ToString(),
                DateOfBirth = new DateTime(_random.Next(1930, 2020), _random.Next(1, 12), _random.Next(1, 32)),
                Gender = "N/N",
                User = user,
                SpecialtyID = _context.Specialties.FirstOrDefault().Id,
                Specialty = _context.Specialties.FirstOrDefault(),
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,

            });
        }

    }
}
