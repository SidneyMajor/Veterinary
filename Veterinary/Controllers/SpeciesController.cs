using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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





        // GET: species/AddOrEdit/5
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var model = new Species();
                return PartialView("_AddOrEditPartial", model);
            }
            else
            {

                var species = await _speciesRepository.GetByIdAsync(id);
                if (species == null)
                {
                    return Json(new
                    {
                        isValid = "error",
                        mensage = "Species Not Found!"
                    });
                }
                return PartialView("_AddOrEditPartial", species);
            }


        }



        // POST: species/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Species species)
        {

            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    try
                    {
                        await _speciesRepository.CreateAsync(species);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "Theres is already a species with that description!",
                                model = species
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = ex.InnerException.Message,
                                model = species
                            });
                        }
                    }

                }
                else
                {
                    try
                    {
                        await _speciesRepository.UpdateAsync(species);

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await _speciesRepository.ExistAsync(species.Id))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "There is not exist that species registed",
                                model = species
                            });
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var upspecies = await _speciesRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    species = Newtonsoft.Json.JsonConvert.SerializeObject(upspecies)
                });
            }

            return PartialView("_AddOrEditPartial", species);
        }

        // POST: species/Delete/5       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Species Not Found!"
                });
            }

            var species = await _speciesRepository.GetByIdAsync(id.Value);
            if (species == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Species Not Found!"
                });
            }

            try
            {
                await _speciesRepository.DeleteAsync(species);
                var upspecies = await _speciesRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    species = Newtonsoft.Json.JsonConvert.SerializeObject(upspecies)
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _speciesRepository.ExistAsync(species.Id))
                {
                    return Json(new
                    {
                        isValid = "failed",
                        mensage = "There is not exist that species registed"
                    });
                }
                else
                {
                    throw;
                }
            }
        }




    }
}
