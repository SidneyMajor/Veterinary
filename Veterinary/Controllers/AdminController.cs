﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IUserHelper _userHelper;
        private readonly IClientRepository _clientRepository;
        //private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        //private readonly IMailHelper _mailHelper;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAnimalRepository _animalRepository;
        private readonly ISpeciesRepository _speciesRepository;
        private readonly IAppointmentRepsitory _appointmentRepsitory;
        private readonly ICombosHelper _combosHelper;

        public AdminController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper, IClientRepository clientRepository
           /*IImageHelper imageHelper*/, IConverterHelper converterHelper,
            /*IMailHelper mailHelper,*/ ISpecialtyRepository specialtyRepository,
            IDoctorRepository doctorRepository, IAnimalRepository animalRepository,
            ISpeciesRepository speciesRepository, IAppointmentRepsitory appointmentRepsitory,
            ICombosHelper combosHelper)
        {
            _documentTypeRepository = documentTypeRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            //_imageHelper = imageHelper;
            _converterHelper = converterHelper;
            //_mailHelper = mailHelper;
            _specialtyRepository = specialtyRepository;
            _doctorRepository = doctorRepository;
            _animalRepository = animalRepository;
            _speciesRepository = speciesRepository;
            _appointmentRepsitory = appointmentRepsitory;
            _combosHelper = combosHelper;
        }


        public async Task<IActionResult> Index()
        {
            var model = new AdminViewModel
            {
                GetDocumentTypes = await _documentTypeRepository.GetAll().ToListAsync(),
                GetSpecialties = await _specialtyRepository.GetAll().ToListAsync(),
                GetSpecies = await _speciesRepository.GetAll().ToListAsync(),
                GetAppointments = await _appointmentRepsitory.GetAll().Include(a => a.Doctor).Include(a=> a.Animal).Include(a=>a.Specialty).Where(a => a.StartTime.Date == DateTime.Today.Date).ToListAsync(),
                NClients = _clientRepository.GetAll().Count(),
                NDoctors = _doctorRepository.GetAll().Count(),
                NAnimals = _animalRepository.GetAll().Count(),
                NAppointments = _appointmentRepsitory.GetAll().Count(),
                DeleteAnimals= _animalRepository.AnimalsDelete(),
                DeleteClients=_clientRepository.ClientsDelete(),
                DeleteDoctors=_doctorRepository.DoctorsDelete(),
            };
            return View(model);
        }

        // GET: Clients only for admin
        //TODO: tenho que trabalhar a view de modo apenas mostrar os btns apagar e detalhes. criar tbm uma para mostrar os utilizadores inativos.

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

            var client = await _clientRepository.GetByIdAsync(id.Value);
            
            if (client == null)
            {
                return NotFound();
            }
            var documentType = await _documentTypeRepository.GetByIdAsync(client.DocumentTypeID);
            client.DocumentType = documentType;
            var user = await _userHelper.GetUserByClientIdAsync(client.Id);
            var animals = await _animalRepository.GetAllAnimalAsync(user.Email);
            client.User = user;
            var model = new AdminClientDetailsViewModel
            {
                GetClient = client,
                GetAnimals = animals,
            };
            return View(model);
        }


        // GET: Cliente/Delete/5
        public async Task<IActionResult> DeleteClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _clientRepository.GetByIdAsync(id.Value);
           
            if (model == null)
            {
                return NotFound();
            }

            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;

            var user = await _userHelper.GetUserByClientIdAsync(model.Id);

            if (await _appointmentRepsitory.CheckAppointmentUserdAsync(user))
            {
                return RedirectToAction(nameof(ListClient));
            }

            return View(model);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("DeleteClient")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _clientRepository.GetByIdAsync(id);
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;
            await _clientRepository.DeleteAsync(model);
            return RedirectToAction(nameof(ListClient));
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

            if (client == null)
            {
                return NotFound();
            }

            var viewmodel = new AdminAnimalDetailViewModel
            {
                Client = client,
                Animal = model,
                GetAppointments=await _appointmentRepsitory.GetUserAppointmentDetailAsync(model.Id,model.User.Email),
            };
            return View(viewmodel);
        }


        public JsonResult GetDoctors(int specialtyId)
        {
            var doctors = _doctorRepository.GetDoctorsSpecialtyId(specialtyId);
            return this.Json(doctors.OrderBy(c => c.FirstName));
        }



    }
}
