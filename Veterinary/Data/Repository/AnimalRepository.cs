using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Helpers;

namespace Veterinary.Data.Repository
{
    public class AnimalRepository : GenericRepository<Animal>, IAnimalRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public AnimalRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Animal>> GetAllAnimalAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin") || await _userHelper.IsUserInRoleAsync(user, "Doctor"))
            {
                return _context.Animals.Where(a => a.WasDeleted == false)
                    .Include(a => a.User)
                    .Include(a => a.Species)
                    .OrderByDescending(a => a.Name);
            }

            return _context.Animals.Include(o => o.Species)
                .Where(a => a.User == user && a.WasDeleted == false)
                .OrderByDescending(o => o.Name);
        }

        public async Task<Animal> GetDetailAnimalAsync(int id, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                var animals = _context.Animals.Include(u => u.User);
                return await animals.FirstOrDefaultAsync(a => a.Id == id && a.WasDeleted == false);
            }

            return await _context.Animals.FirstOrDefaultAsync(a => a.User == user && a.Id == id && a.WasDeleted == false);
        }
    }
}
