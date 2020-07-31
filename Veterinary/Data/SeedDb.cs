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

            if (!_context.Clients.Any())
            {

                var user = await _userHelper.GetUserByEmailAsync("Sidney.major@seed.pt");

                var user1 = await _userHelper.GetUserByEmailAsync("Isabel@seed.pt");

                if (user == null)
                {
                    user = new User
                    {
                        Email = "Sidney.major@seed.pt",
                        UserName = "Sidney.major@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(user, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                if (user1 == null)
                {
                    user1 = new User
                    {
                        Email = "Isabel@seed.pt",
                        UserName = "Isabel@seed.pt",
                    };
                    var result = await _userHelper.AddUserAsync(user1, "123456");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create the user in seeder");
                    }
                }

                this.AddClient("Sidney", user);
                this.AddClient("Isabel", user1);

                await _context.SaveChangesAsync();

            }
        }

        private void AddClient(string name, User user)
        {


            _context.Clients.Add(new Client
            {
                FirstName = name,
                LastName = "Major",
                Address = "Rua dos milagres",
                DocumentTypeID = _context.DocumentTypes.FirstOrDefault().Id,
                DocumentType = _context.DocumentTypes.FirstOrDefault(),
                Document = _random.Next(10000, 999999).ToString(),
                TaxNumber = _random.Next(100000000, 399999999).ToString(),
                DateOfBirth = new DateTime(_random.Next(1930, 2020), _random.Next(1, 12), _random.Next(1, 32)),
                Gender="N/N",
                User = user,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
            });
        }


    }
}
