using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> RegisterDoctor()
        {
            var model = new RegisterNewDoctorViewModel
            {
                Specialties = await _specialtyRepository.GetComboSpecialties(),
                Documents = await _documentTypeRepository.GetComboDocuments(),
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

                try
                {
                    var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);
                    var specialty = await _specialtyRepository.GetByIdAsync(model.SpecialtyID);
                    var user = await _userHelper.GetUserByEmailAsync(model.Email);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                        };

                        var result = await _userHelper.AddUserAsync(user, "");
                        if (result != IdentityResult.Success)
                        {
                            this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                            model.Documents = await _documentTypeRepository.GetComboDocuments();
                            model.Specialties = await _specialtyRepository.GetComboSpecialties();
                            return this.View(model);
                        }
                    }

                    var doctor = await _doctorRepository.GetDoctorByUserEmailAsync(model.Email);

                    if (doctor == null)
                    {
                        //add doctor
                        await _userHelper.CheckRoleAsync("Doctor");
                        await _userHelper.AddUserToRoleAsync(user, "Doctor");

                        doctor = _converterHelper.ToDoctor(model, documentType, specialty, path);
                        doctor.User = user;
                        await _doctorRepository.CreateAsync(doctor);

                        var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                        var tokenLink = this.Url.Action("ConfirmDoctorEmail", "Account", new
                        {
                            userid = user.Id,
                            token = myToken,
                            name = $"{doctor.FirstName} {doctor.LastName}",

                        }, protocol: HttpContext.Request.Scheme);

                        try
                        {
                           await _mailHelper.SendMail(model.Email, "Email confirmation", $"<center><h1 style=\"margin:20px\">Email Confirmation</h1></center>" + "<div style=\"padding: 0 2.5em; text-align: center;\">" +
                                 "<h1> To allow the user</h1>	" + $"<a href=\"{tokenLink}\" style='display: inline-block;font-weight: 600; " +
                                 $"color: aliceblue;text-align: center;vertical-align: middle;-webkit-user-select: none;-moz-user-select: none;-ms-user-select: none;" +
                                 $"user-select: none;background-color: transparent;border: 1px solid transparent;padding: 0.375rem 0.75rem;font-size: 1rem;" +
                                 $"line-height: 2.5;border-radius: 0.25rem;transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out, border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out; " +
                                 $"background-color: #008CBA;'>Confirm your account</a></div>");
                        }
                        catch (Exception)
                        {

                           
                        } 

                        this.ViewBag.Message = "The instructions to allow your user has been sent to email.";
                        model.Documents = await _documentTypeRepository.GetComboDocuments();
                        model.Specialties = await _specialtyRepository.GetComboSpecialties();
                        return this.View(model);
                    }

                    this.ModelState.AddModelError(string.Empty, "The user already exists.");

                }
                catch (Exception ex)
                {

                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Theres is already a doctor with that tax number, nº document or social security number !!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }



            }

            model.Documents = await _documentTypeRepository.GetComboDocuments();
            model.Specialties = await _specialtyRepository.GetComboSpecialties();
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

            Doctor doctor = await _doctorRepository.GetByIdAsync(id.Value);

            if (doctor == null)
            {
                return NotFound();
            }
            doctor.DocumentType = await _documentTypeRepository.GetByIdAsync(doctor.DocumentTypeID);
            doctor.Specialty = await _specialtyRepository.GetByIdAsync(doctor.SpecialtyID);
            doctor.User = await _userHelper.GetUserByDoctorIdAsync(doctor.Id);

            var appointments = await _appointmentRepsitory.GetAllAppointmentlAsync(doctor.User.Email);
            var model = new DoctorDetailsViewModel
            {
                GetDoctor = doctor,
                GetAppointments = appointments,
            };
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
            model.GetClient = await _clientRepository.GetClientByUserEmailAsync(username);
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
                return Json(new { myappointments = Newtonsoft.Json.JsonConvert.SerializeObject(myupappointments.Where(a => a.Status == "Accepted")) });

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

    }
}
