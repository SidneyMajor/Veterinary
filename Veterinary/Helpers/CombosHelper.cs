using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Veterinary.Data;
using Veterinary.Data.Entities;

namespace Veterinary.Helpers
{
    public class CombosHelper : ICombosHelper, IDisposable
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboAnimals(IQueryable<Animal> animals)
        {
            var list = animals.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select an Animal...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDoctors(int id)
        {
            var list = _context.Doctors.Where(s => s.SpecialtyID == id && s.WasDeleted==false).Select(d => new SelectListItem
            {
                Text = d.FullName,
                Value = d.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Veterinary...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboSpecialties()
        {
            var list = _context.Specialties.Where(s=> s.WasDeleted==false).Select(s =>  new SelectListItem
            {
                Text = s.Description,
                Value = s.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Specialty...)",
                Value = "0"
            });

            return list;
        }


        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
