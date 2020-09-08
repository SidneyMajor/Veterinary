using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinary.Data;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IUserHelper _userHelper;
        private readonly IClientRepository _clientRepository;
        //private readonly IImageHelper _imageHelper;
        //private readonly IConverterHelper _converterHelper;
        //private readonly IMailHelper _mailHelper;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IAppointmentRepsitory _appointmentRepsitory;
        private readonly DataContext contex;

        public AdminController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper, IClientRepository clientRepository,
            //IImageHelper imageHelper, IConverterHelper converterHelper,
            /*IMailHelper mailHelper,*/ ISpecialtyRepository specialtyRepository,
            IDoctorRepository doctorRepository, IAnimalRepository animalRepository,
            ISpeciesRepository speciesRepository, IAppointmentRepsitory appointmentRepsitory,
            DataContext contex)
        {
            _documentTypeRepository = documentTypeRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            //_imageHelper = imageHelper;
            //_converterHelper = converterHelper;
            //_mailHelper = mailHelper;
            _specialtyRepository = specialtyRepository;
            _doctorRepository = doctorRepository;
            _animalRepository = animalRepository;
            _speciesRepository = speciesRepository;
            _appointmentRepsitory = appointmentRepsitory;
            this.contex = contex;
        }


        public IActionResult Index()
        {
            return View();
        }



        // GET: Clients only for admin
        //TODO: tenho que trabalhar a view de modo apenas mostrar os btns apagar e detalhes. criar tbm uma para mostrar os utilizadores inativos.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListClient()
        {
            var userAdmin = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var isInRoleAdmin = await _userHelper.IsUserInRoleAsync(userAdmin, "Admin");
            if (isInRoleAdmin)
            {
                return View(await _clientRepository.GetAll().Where(c => c.User != userAdmin).Include(u => u.User).ToListAsync());
            }
            return View(await _clientRepository.GetAll().Include(u => u.User).ToListAsync());
        }

        // GET: Client/Details/5        
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _clientRepository.GetByIdAsync(id.Value);
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        // GET: Cliente/Delete/5
        public async Task<IActionResult> DeleteCliente(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _clientRepository.GetByIdAsync(id.Value);
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("DeleteCliente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _clientRepository.GetByIdAsync(id);
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;
            await _clientRepository.DeleteAsync(model);
            return RedirectToAction(nameof(ListClient));
        }


        public async Task<IActionResult> GetAnimalByClientId(int id)
        {
            var user = await _userHelper.GetUserByClientIdAsync(id);

            return View(await _animalRepository.GetAllAnimalAsync(user.Email));
        }

        public async Task<IActionResult> ListAnimal()
        {
            return View(await _animalRepository.GetAllAnimalAsync(this.User.Identity.Name));
        }

        // GET: Client/Details/5        
        public async Task<IActionResult> AnimalDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _animalRepository.GetDetailAnimalAsync(id.Value, this.User.Identity.Name);
            if (model == null)
            {
                return NotFound();
            }

            model.Species = await _speciesRepository.GetByIdAsync(model.SpeciesID);

            var client = await _clientRepository.GetClientByUserEmailAsync(model.User.Email);

            if (client==null)
            {
                return NotFound();
            }

            var viewmodel = new AdminAnimalDetailViewModel
            {
                Client = client,
                Animal = model
            };
            return View(viewmodel);
        }


        public IActionResult AdminAppointment()
        {
            IEnumerable<Appointment> appointments = new List<Appointment>();
            appointments =  this.contex.Appointments.Include(a => a.Animal).Include(a => a.Doctor);
            foreach (var item in appointments)
            {
                item.Subject = item.Animal.Name;
            }
            ViewBag.appointments = appointments.ToList();

            return View();
        }
    }
}
