using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Veterinary.Data;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;

namespace Veterinary.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpecialtiesController : Controller
    {
        private readonly DataContext _context;
        private readonly ISpecialtyRepository _specialtyRepository;

        public SpecialtiesController(DataContext context, ISpecialtyRepository specialtyRepository)
        {
            _context = context;
            _specialtyRepository = specialtyRepository;
        }



        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            return View(await _specialtyRepository.GetAll().ToListAsync());
        }



        // GET: Specialty/AddOrEdit/5
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var model = new Specialty();
                return PartialView("_AddOrEditPartial", model);
            }
            else
            {

                var specialty = await _specialtyRepository.GetByIdAsync(id);
                if (specialty == null)
                {
                    return Json(new
                    {
                        isValid = "error",
                        mensage = "Specialty Not Found!"
                    });
                }
                return PartialView("_AddOrEditPartial", specialty);
            }


        }



        // POST: Specialty/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Specialty specialty)
        {

            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    try
                    {
                        await _specialtyRepository.CreateAsync(specialty);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "Theres is already a Specialty with that description!",
                                model = specialty
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = ex.InnerException.Message,
                                model = specialty
                            });
                        }
                    }

                }
                else
                {
                    try
                    {
                        await _specialtyRepository.UpdateAsync(specialty);

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await _specialtyRepository.ExistAsync(specialty.Id))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "There is not exist that Specialty registed",
                                model = specialty
                            });
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var upspecialties = await _specialtyRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    specialties = Newtonsoft.Json.JsonConvert.SerializeObject(upspecialties)
                });
            }

            return PartialView("_AddOrEditPartial", specialty);
        }

        // POST: Specialty/Delete/5       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Specialty Not Found!"
                });
            }

            var specialty = await _specialtyRepository.GetByIdAsync(id.Value);
            if (specialty == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Specialty Not Found!"
                });
            }

            try
            {
                await _specialtyRepository.DeleteAsync(specialty);
                var upspecialties = await _specialtyRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    specialties = Newtonsoft.Json.JsonConvert.SerializeObject(upspecialties)
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _specialtyRepository.ExistAsync(specialty.Id))
                {
                    return Json(new
                    {
                        isValid = "failed",
                        mensage = "There is not exist that Specialty registed"
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
