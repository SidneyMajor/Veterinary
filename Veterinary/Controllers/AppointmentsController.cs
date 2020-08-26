using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public AppointmentsController(IAppointmentRepsitory appointmentRepsitory,
            IAnimalRepository animalRepository,
            IDoctorRepository doctorRepository,IConverterHelper converterHelper,
            IUserHelper userHelper,DataContext context)
        {
            _appointmentRepsitory = appointmentRepsitory;
            _animalRepository = animalRepository;
            _doctorRepository = doctorRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            //var dataContext = _context.Appointment.Include(a => a.Animal).Include(a => a.Doctor).ThenInclude(a=> a.User);
            return View(await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name));
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(appointment);
        }

       

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["AnimalID"] = new SelectList(_context.Animals, "Id", "Id", appointment.AnimalID);
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "Id", "Address", appointment.DoctorID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["AnimalID"] = new SelectList(_context.Animals, "Id", "Id", appointment.AnimalID);
            ViewData["DoctorID"] = new SelectList(_context.Doctors, "Id", "Address", appointment.DoctorID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Animal)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
