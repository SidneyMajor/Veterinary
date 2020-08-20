using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.DropDowns;
using Veterinary.Data;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    [Authorize(Roles ="Owner")]
    public class AnimalsController : Controller
    {
        private readonly DataContext _context;
        private readonly IAnimalRepository _animalRepository;
        private readonly IUserHelper _userHelper;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public AnimalsController(DataContext context, IAnimalRepository animalRepository, IUserHelper userHelper,
            ISpeciesRepository speciesRepository,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _context = context;
            _animalRepository = animalRepository;
            _userHelper = userHelper;
            _speciesRepository = speciesRepository;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Animals
        //Todo: usar uma data table
        public async Task<IActionResult> Index()
        {
            //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            //var dataContext = await _animalRepository.GetAll().Where(u => u.User == user).Include(a => a.Species).ToListAsync();
            return View(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name));
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            //if (user == null)
            //{
            //    return NotFound();
            //}
            
            var animal = await _animalRepository.GetDetailAnimalAsync(id.Value, this.User.Identity.Name);
            animal.Species = await _speciesRepository.GetByIdAsync(animal.SpeciesID);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            var model = new AnimalViewModel
            {
                GetSpecies = _speciesRepository.GetAll().ToList(),
            };
            return View(model);
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    if (_imageHelper.ValidFileTypes(model.ImageFile))
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Animals");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "Invalid File. Please upload a File with extension (bmp, gif, png, jpg, jpeg)");
                        return this.View(model);
                    }                   
                }

                var species = await _speciesRepository.GetByIdAsync(model.SpeciesID);
                var animal = _converterHelper.ToAnimal(model, species, path, true);
                animal.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _animalRepository.CreateAsync(animal);
                return RedirectToAction(nameof(Index));
            }
            model.GetSpecies = _speciesRepository.GetAll().ToList();
            return View(model);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);
            if (animal == null)
            {
                return NotFound();
            }
            var species = await _speciesRepository.GetByIdAsync(animal.SpeciesID);
            var model = _converterHelper.ToRegisterNewAnimalViewModel(animal, species);
            model.GetSpecies = _speciesRepository.GetAll().ToList();
            //ViewData["SpeciesID"] = new SelectList(_context.Species, "Id", "Id", animal.SpeciesID);
            return View(model);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  AnimalViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var path =model.ImageUrl;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        if (_imageHelper.ValidFileTypes(model.ImageFile))
                        {
                            path = await _imageHelper.UploadImageAsync(model.ImageFile, "Animals"); 
                        }
                        else
                        {
                            this.ModelState.AddModelError(string.Empty, "Invalid File. Please upload a File with extension (bmp, gif, png, jpg, jpeg)");
                            return this.View(model);
                        }
                    }

                    var species = await _speciesRepository.GetByIdAsync(model.SpeciesID);

                    var animal = _converterHelper.ToAnimal(model, species, path, false);
                    animal.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    await _animalRepository.UpdateAsync(animal);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _animalRepository.ExistAsync(model.Id))
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
            model.GetSpecies = _speciesRepository.GetAll().ToList();
            return View(model);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _animalRepository.GetByIdAsync(id.Value);
            animal.Species = await _speciesRepository.GetByIdAsync(animal.SpeciesID);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _animalRepository.GetByIdAsync(id);
            await _animalRepository.DeleteAsync(animal);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
