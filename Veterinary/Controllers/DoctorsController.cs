using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public DoctorsController(IDoctorRepository doctorRepository, ISpecialtyRepository specialtyRepository,
            IUserHelper userHelper, IImageHelper imageHelper, IDocumentTypeRepository documentTypeRepository,
            IConverterHelper converterHelper, IMailHelper mailHelper)
        {
            _doctorRepository = doctorRepository;
            _specialtyRepository = specialtyRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _documentTypeRepository = documentTypeRepository;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
        }
        public IActionResult ListDoctor()
        {
            return View(_doctorRepository.GetAll().ToList());
        }


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


                    var result = await _userHelper.AddUserAsync(user,"");
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
                    var tokenLink = this.Url.Action("ConfirmDoctorEmail", "Doctors", new
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


        //Todo: melhorar essa view
        public async Task<IActionResult> ConfirmDoctorEmail(string userid, string token)
        {
            if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            var user = await _userHelper.GetUserByIdAsync(userid);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return NotFound();
            }

            var model = new SetPasswordViewModel { UserId = userid };
            return View(model);


        }



        [HttpPost]
        public async Task<IActionResult> ConfirmDoctorEmail(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByIdAsync(model.UserId);

                if (user != null)
                {
                    var result = await _userHelper.AddPasswordAsync(user, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");
                }
            }

            return View(model);

        }



        // GET: doctor/Details       
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


    }
}
