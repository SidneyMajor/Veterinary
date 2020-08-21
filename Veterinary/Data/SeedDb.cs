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
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Owner");

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

            if (!_context.Clients.Any())
            {

                var userAdmin = await _userHelper.GetUserByEmailAsync("Sidney.major@seed.pt");

                var userOwner = await _userHelper.GetUserByEmailAsync("Isabel@seed.pt");

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

                if (userOwner == null)
                {
                    userOwner = new User
                    {
                        Email = "Isabel@seed.pt",
                        UserName = "Isabel@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(userOwner, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                var isInRoleAdmin = await _userHelper.IsUserInRoleAsync(userAdmin, "Admin");
                var isInRoleOwner = await _userHelper.IsUserInRoleAsync(userOwner, "Admin");

                if (!isInRoleAdmin)
                {
                    await _userHelper.AddUserToRoleAsync(userAdmin, "Admin");
                }

                if (!isInRoleOwner)
                {
                    await _userHelper.AddUserToRoleAsync(userOwner, "Owner");
                }
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(userAdmin);
                await _userHelper.ConfirmEmailAsync(userAdmin, token);

                var tokenOwner = await _userHelper.GenerateEmailConfirmationTokenAsync(userOwner);
                await _userHelper.ConfirmEmailAsync(userOwner, tokenOwner);

                this.AddClient("Sidney", "Major", userAdmin);
                this.AddClient("Isabel", "Frazão", userOwner);

                this.AddAnimal("Piter", userOwner);
                this.AddAnimal("Xana", userOwner);

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
                City="Loures",
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
                DateOfBirth = new DateTime(_random.Next(2000, 2020), _random.Next(1, 12), _random.Next(1, 32)),
                Gender = "F",
                SpeciesID= _context.Species.FirstOrDefault().Id,
                Species= _context.Species.FirstOrDefault(),
                User = user,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
            });
        }


    }
}
