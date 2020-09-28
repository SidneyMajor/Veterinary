using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Veterinary.Data.Entities;

namespace Veterinary.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboAnimals(IQueryable<Animal> animals);

        IEnumerable<SelectListItem> GetComboSpecialties();

        IEnumerable<SelectListItem> GetComboDoctors(int id);
    }
}
