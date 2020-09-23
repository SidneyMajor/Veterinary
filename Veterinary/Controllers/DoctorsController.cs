using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{

    public class DoctorsController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IAppointmentRepsitory _appointmentRepsitory;
        private readonly ICombosHelper _combosHelper;
        private readonly IAnimalRepository _animalRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ISpeciesRepository _speciesRepository;

        public DoctorsController(IDoctorRepository doctorRepository, ISpecialtyRepository specialtyRepository,
            IUserHelper userHelper, IImageHelper imageHelper, IDocumentTypeRepository documentTypeRepository,
            IConverterHelper converterHelper, IMailHelper mailHelper, IAppointmentRepsitory appointmentRepsitory, 
            ICombosHelper combosHelper, IAnimalRepository animalRepository, IClientRepository clientRepository, ISpeciesRepository speciesRepository)
        {
            _doctorRepository = doctorRepository;
            _specialtyRepository = specialtyRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _documentTypeRepository = documentTypeRepository;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
            _appointmentRepsitory = appointmentRepsitory;
            _combosHelper = combosHelper;
            _animalRepository = animalRepository;
            _clientRepository = clientRepository;
           _speciesRepository = speciesRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListDoctor()
        {
            return View(_doctorRepository.GetAll().Include(d => d.User).ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RegisterDoctor()
        {
            var model = new RegisterNewDoctorViewModel
            {
                Specialties = _specialtyRepository.GetAll().ToList(),
                Documents = _documentTypeRepository.GetAll().ToList(),
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(RegisterNewDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    if (_imageHelper.ValidFileTypes(model.ImageFile))
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Doctors");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "Invalid File. Please upload a File with extension (bmp, gif, png, jpg, jpeg)");
                        return this.View(model);
                    }

                }

                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                    };

                    var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
                    var specialty = await _specialtyRepository.GetByIdAsync(model.SpecialtyID);


                    var result = await _userHelper.AddUserAsync(user, "");
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        model.Documents = _documentTypeRepository.GetAll().ToList();
                        model.Specialties = _specialtyRepository.GetAll().ToList();
                        return this.View(model);
                    }
                    //add doctor

                    await _userHelper.CheckRoleAsync("Doctor");
                    await _userHelper.AddUserToRoleAsync(user, "Doctor");

                    var doctor = _converterHelper.ToDoctor(model, documentType, specialty, path);
                    doctor.User = user;
                    await _doctorRepository.CreateAsync(doctor);



                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = this.Url.Action("ConfirmDoctorEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken,

                    }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                       $"To allow the user, " +
                       $"plase click in this link: </br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                    return this.View(model);
                }
                this.ModelState.AddModelError(string.Empty, "The user already exists.");
            }

            model.Documents = _documentTypeRepository.GetAll().ToList();
            model.Specialties = _specialtyRepository.GetAll().ToList();
            return this.View(model);
        }



        // GET: doctor/Details  
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DoctorDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _doctorRepository.GetByIdAsync(id.Value);

            if (model == null)
            {
                return NotFound();
            }
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            var specialty = await _specialtyRepository.GetByIdAsync(model.SpecialtyID);
            model.DocumentType = documentType;
            model.Specialty = specialty;
            return View(model);
        }


        // GET: Cliente/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _doctorRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            var specialty = await _specialtyRepository.GetByIdAsync(model.SpecialtyID);
            model.DocumentType = documentType;
            model.Specialty = specialty;

            return View(model);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await _doctorRepository.GetByIdAsync(id);
            var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
            model.DocumentType = documentType;
            await _doctorRepository.DeleteAsync(model);
            return RedirectToAction(nameof(ListDoctor));
        }

        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> MyAppointment()
        {
            return View(await _appointmentRepsitory.DoctorAppointmentsAsync(this.User.Identity.Name));
        }


        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> StartAppointment(int? id, string username)
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

            var model = _converterHelper.ToDoctorAppointmentViewModel(appointment);
            model.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
            model.Animal.Species = await _speciesRepository.GetByIdAsync(model.Animal.SpeciesID);
            model.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);
            model.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);
            model.GetClient= await _clientRepository.GetClientByUserEmailAsync(username);
            model.GetClient.User = await _userHelper.GetUserByEmailAsync(username);
            return PartialView("_startAppointmentPartial", model);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> StartAppointment(int id, DoctorAppointmentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            Appointment appointment = _converterHelper.ToAppointment(model);
            var user = await _userHelper.GetUserByAnimalIdAsync(model.AnimalID);

            appointment.User = user;
            appointment.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
            appointment.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);
            appointment.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);

            try
            {
               
                await _appointmentRepsitory.UpdateAsync(appointment);
                var myupappointments = await _appointmentRepsitory.DoctorAppointmentsAsync(this.User.Identity.Name);
                return Json(new { myappointments = Newtonsoft.Json.JsonConvert.SerializeObject(myupappointments) });

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
            //model = _converterHelper.ToDoctorAppointmentViewModel(appointment);
            //model.Animal = await _animalRepository.GetByIdAsync(appointment.AnimalID);
            //model.Specialty = await _specialtyRepository.GetByIdAsync(appointment.SpecialtyID);
            //model.Doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorID);

            //return PartialView("_startAppointmentPartial", model);
        }

    }
}
