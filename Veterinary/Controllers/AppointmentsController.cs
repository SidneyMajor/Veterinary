using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly ISpecialtyRepository _specialtyRepository;

        public AppointmentsController(IAppointmentRepsitory appointmentRepsitory,
            IAnimalRepository animalRepository,
            IDoctorRepository doctorRepository, IConverterHelper converterHelper,
            IUserHelper userHelper, ISpecialtyRepository specialtyRepository, DataContext context)
        {
            _appointmentRepsitory = appointmentRepsitory;
            _animalRepository = animalRepository;
            _doctorRepository = doctorRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _context = context;
            _specialtyRepository = specialtyRepository;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
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



        // GET: Appointments/Create
        public async Task<IActionResult> CreateAppointment()
        {
            var model = new NewAppointmentViewModel
            {
                Animals = await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name),
                Specialties = await _specialtyRepository.GetAll().ToListAsync(),
                Doctors = await _doctorRepository.GetAll().ToListAsync(),
            };
            ViewBag.Enabled = false;
            return View(model);
        }




        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(NewAppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Todo: Fazer as validaçoes necessarias antes de criar uma consulta
                Appointment appointment = _converterHelper.ToAppointment(model, true);
                appointment.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                appointment.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
                appointment.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);
                appointment.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);
                //var teste = await _appointmentRepsitory.CheckAppointmentAsync(appointment);
                if (!await _appointmentRepsitory.CheckAppointmentAsync(appointment))
                {
                    await _appointmentRepsitory.CreateAsync(appointment);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "The appointment you have chosen is no longer available. Please choose another time.");
            }
            model.Animals = await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name);
            model.Doctors = await _doctorRepository.GetAll().ToListAsync();
            model.Specialties = await _specialtyRepository.GetAll().ToListAsync();
            ViewBag.Enabled = true;
            return View(model);
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
