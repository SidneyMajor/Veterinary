using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class ClientsController : Controller
    {

        private readonly IAppointmentRepsitory _appointmentRepsitory;
        private readonly IAnimalRepository _animalRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;


        public ClientsController(IAppointmentRepsitory appointmentRepsitory,
            IAnimalRepository animalRepository,
            IDoctorRepository doctorRepository, IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _appointmentRepsitory = appointmentRepsitory;
            _animalRepository = animalRepository;
            _doctorRepository = doctorRepository;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            //var dataContext = _context.Appointment.Include(a => a.Animal).Include(a => a.Doctor).ThenInclude(a=> a.User);
            return View(await _appointmentRepsitory.GetAllAppointmentlAsync(this.User.Identity.Name));
        }

        // GET: Appointments/Create
        public async Task<IActionResult> CreateAppointment()
        {
            //ViewData["AnimalID"] = new SelectList(_context.Animals, "Id", "Id");
            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "Id", "Address");

            var model = new NewAppointmentViewModel
            {
                Animals = await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name),
                Doctors = await _doctorRepository.GetAll().ToListAsync(),
            };
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
                await _appointmentRepsitory.CreateAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AnimalID"] = new SelectList(_context.Animals, "Id", "Id", appointment.AnimalID);
            //ViewData["DoctorID"] = new SelectList(_context.Doctors, "Id", "Address", appointment.DoctorID);

            model.Animals = await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name);
            model.Doctors = await _doctorRepository.GetAll().ToListAsync();
            return View(model);
        }
    }
}
