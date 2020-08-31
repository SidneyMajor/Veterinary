using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;

namespace Veterinary.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpeciesController : Controller
    {

        private readonly ISpeciesRepository _speciesRepository;

        public SpeciesController(ISpeciesRepository speciesRepository)
        {
            _speciesRepository = speciesRepository;
        }

        // GET: Species
        public async Task<IActionResult> Index()
        {
            return View(await _speciesRepository.GetAll().ToListAsync());
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = await _speciesRepository.GetByIdAsync(id.Value);
            if (species == null)
            {
                return NotFound();
            }

            return View(species);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Species/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Species model)
        {
            if (ModelState.IsValid)
            {
                await _speciesRepository.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _speciesRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Species/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Species model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _speciesRepository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _speciesRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Species/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _speciesRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _speciesRepository.GetByIdAsync(id);
            await _speciesRepository.DeleteAsync(model);
            return RedirectToAction(nameof(Index));
        }

    }
}
