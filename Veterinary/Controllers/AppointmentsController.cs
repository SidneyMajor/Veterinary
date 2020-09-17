﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentRepsitory _appointmentRepsitory;
        private readonly IAnimalRepository _animalRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly ISpecialtyRepository _specialtyRepository;

        public AppointmentsController(IAppointmentRepsitory appointmentRepsitory,
            IAnimalRepository animalRepository,
            IDoctorRepository doctorRepository, IConverterHelper converterHelper,
            IUserHelper userHelper, ISpecialtyRepository specialtyRepository, DataContext context,
             ICombosHelper combosHelper)
        {
            _appointmentRepsitory = appointmentRepsitory;
            _animalRepository = animalRepository;
            _doctorRepository = doctorRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _specialtyRepository = specialtyRepository;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            return View(await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name));
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> ConfirmSchedule(int? id, string status)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentRepsitory.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                appointment.Status = status;
                await _appointmentRepsitory.UpdateAsync(appointment);
                var upappointments = await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name);
                return Json(new { result = status , appointments= Newtonsoft.Json.JsonConvert.SerializeObject(upappointments) });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _appointmentRepsitory.ExistAsync(appointment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
        }

        //Todo: Appointment Status:Pending, Accepted, Canceled
        // GET: Appointments/Create
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(DateTime startTime, DateTime endTime)
        {
            var model = new AppointmentViewModel
            {
                Animals = _combosHelper.GetComboAnimals(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name)),
                Specialties = _combosHelper.GetComboSpecialties(),
                Doctors = _combosHelper.GetComboDoctors(0),
                StartTime = startTime,
                EndTime = endTime,
            };
            return PartialView("_viewCreatePartial", model);
        }



        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Todo: Fazer as validaçoes necessarias antes de criar uma consulta
                Appointment appointment = _converterHelper.ToAppointment(model, true);
                //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                //if (await _userHelper.IsUserInRoleAsync(user,"Admin"))
                //{
                //    user = await _userHelper.GetUserByAnimalIdAsync(model.AnimalID);
                //}
                appointment.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                appointment.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
                appointment.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);
                appointment.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);
                //var teste = await _appointmentRepsitory.CheckAppointmentAsync(appointment);
                if (!await _appointmentRepsitory.CheckAppointmentAsync(appointment))
                {
                    await _appointmentRepsitory.CreateAsync(appointment);
                    //ViewBag.appointments = appointments.ToList();
                    var upappointments = await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name);
                    return Json(new { isValid = "success", message = "To add appointment", appointments = Newtonsoft.Json.JsonConvert.SerializeObject(upappointments) });
                }
                else
                {
                    return Json(new { isValid = "failed", message = "The appointment you have chosen is no longer available. Please choose another time." });
                }

            }
            model.Animals = _combosHelper.GetComboAnimals(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name));
            model.Specialties = _combosHelper.GetComboSpecialties();
            model.Doctors = _combosHelper.GetComboDoctors(model.SpecialtyID);

            return PartialView("_viewCreatePartial", model);
        }


        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Appointment appointment = await _appointmentRepsitory.GetByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToAppointmentViewModel(appointment);
            model.Animals = _combosHelper.GetComboAnimals(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name));
            model.Specialties = _combosHelper.GetComboSpecialties();
            model.Doctors = _combosHelper.GetComboDoctors(appointment.SpecialtyID);

            //ViewData["AnimalID"] = new SelectList(_context.Animals, "Id", "Id", appointment.AnimalID);
            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "Id", "Address", appointment.DoctorID);
            return PartialView("_viewEditPartial", model);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Appointment appointment = _converterHelper.ToAppointment(model, false);
                    //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    //if (await _userHelper.IsUserInRoleAsync(user,"Admin"))
                    //{
                    //    user = await _userHelper.GetUserByAnimalIdAsync(model.AnimalID);
                    //}
                    appointment.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    appointment.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
                    appointment.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);
                    appointment.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);
                    //var teste = await _appointmentRepsitory.CheckAppointmentAsync(appointment);
                    if (!await _appointmentRepsitory.CheckAppointmentAsync(appointment))
                    {
                        await _appointmentRepsitory.UpdateAsync(appointment);
                        //ViewBag.appointments = appointments.ToList();
                        var upappointments = await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name);
                        return Json(new { isValid = "success", message = "To add appointment", appointments = Newtonsoft.Json.JsonConvert.SerializeObject(upappointments) });
                    }
                    else
                    {
                        return Json(new { isValid = "failed", message = "The appointment schedule you have chosen is not available. Please choose another hour or date!" });
                    }
                    //_context.Update(appointment);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _appointmentRepsitory.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
           
            model.Animals = _combosHelper.GetComboAnimals(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name));
            model.Specialties = _combosHelper.GetComboSpecialties();
            model.Doctors = _combosHelper.GetComboDoctors(model.SpecialtyID);

            return PartialView("_viewEditPartial", model);
        }

        // post: Appointments/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { result = "error" });
            }

            var appointment = await _context.Appointments
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return Json(new { result = "error" });
            }
            try
            {
                await _appointmentRepsitory.DeleteAsync(appointment);
                var upappointments = await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name);
                return Json(new { result = "success", appointments =  Newtonsoft.Json.JsonConvert.SerializeObject(upappointments) });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _appointmentRepsitory.ExistAsync(id.Value))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        //// POST: Appointments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var appointment = await _context.Appointments.FindAsync(id);
        //    _context.Appointments.Remove(appointment);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

      


        public JsonResult GetDoctors(int specialtyId)
        {
            var doctors = _doctorRepository.GetDoctorsSpecialtyId(specialtyId);
            return this.Json(doctors.OrderBy(c => c.FirstName));
        }
    }
}
