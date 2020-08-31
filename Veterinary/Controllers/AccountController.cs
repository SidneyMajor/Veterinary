using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;
using Veterinary.Helpers;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IUserHelper _userHelper;
        private readonly IClientRepository _clientRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AccountController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper, IClientRepository clientRepository,
            IImageHelper imageHelper, IConverterHelper converterHelper,
            IMailHelper mailHelper, ISpecialtyRepository specialtyRepository,
            IDoctorRepository doctorRepository)
        {
            _documentTypeRepository = documentTypeRepository;
            _userHelper = userHelper;
            _clientRepository = clientRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
            _specialtyRepository = specialtyRepository;
            _doctorRepository = doctorRepository;
        }


        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        //Direção de retorno
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return this.RedirectToAction("Index", "Home");
                }

            }

            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return this.View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }


       
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Documents = _documentTypeRepository.GetAll().ToList()
            };

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    if (_imageHelper.ValidFileTypes(model.ImageFile))
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Clients");
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

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        model.Documents = _documentTypeRepository.GetAll().ToList();

                        return this.View(model);
                    }

                    //add client
                    await _userHelper.CheckRoleAsync("Customer");
                    await _userHelper.AddUserToRoleAsync(user, "Customer");

                    var client = _converterHelper.ToClient(model, documentType, path);
                    client.User = user;
                    await _clientRepository.CreateAsync(client);


                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
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
            return this.View(model);
        }



        //Todo: melhorar essa view
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
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
            ////teste
            //if (await _userHelper.IsUserInRoleAsync(user, "Doctor"))
            //{
            //    var model = new SetPasswordViewModel { UserId = userid };
            //    return View(model);
            //}


            return View();
        }



        // GET: Client
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);


            if (await _userHelper.IsUserInRoleAsync(user, "Doctor"))
            {
                var doctor = await _doctorRepository.GetDoctorByUserEmailAsync(this.User.Identity.Name);
                if (doctor == null)
                {
                    //model.FirstName = client.FirstName;
                    //model.LastName = client.LastName;
                    //model.Address = client.Address;
                    //model.ZipCode = client.ZipCode;
                    //model.PhoneNumber = client.PhoneNumber;
                    //model.TaxNumber = client.TaxNumber;
                    //model.Gender = client.Gender;
                    //model.DateOfBirth = client.DateOfBirth.Date;
                    //model.Nationality = client.Nationality;
                    //model.Document = client.Document;                
                    return NotFound();

                }

                //var documentType = await _documentTypeRepository.GetByIdAsync(doctor.DocumentTypeID);
                //var specialty = await _specialtyRepository.GetByIdAsync(doctor.SpecialtyID);
                var model = _converterHelper.ToChangeUserViewModel(doctor);
                model.Documents = _documentTypeRepository.GetAll();
                model.Specialties = _specialtyRepository.GetAll();
                return this.View(model);
            }
            else
            {
                var client = await _clientRepository.GetClientByUserEmailAsync(this.User.Identity.Name);
                if (client == null)
                {
                    //model.FirstName = client.FirstName;
                    //model.LastName = client.LastName;
                    //model.Address = client.Address;
                    //model.ZipCode = client.ZipCode;
                    //model.PhoneNumber = client.PhoneNumber;
                    //model.TaxNumber = client.TaxNumber;
                    //model.Gender = client.Gender;
                    //model.DateOfBirth = client.DateOfBirth.Date;
                    //model.Nationality = client.Nationality;
                    //model.Document = client.Document;                
                    return NotFound();

                }

                //var documentType = await _documentTypeRepository.GetByIdAsync(client.DocumentTypeID);
                var model = _converterHelper.ToChangeUserViewModel(client);
                model.Documents = _documentTypeRepository.GetAll();
                return this.View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {

            if (this.ModelState.IsValid)
            {

                try
                {
                    var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);

                    var path = model.ImageUrl;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        //Todo: Fazer com JS essa validação.
                        if (_imageHelper.ValidFileTypes(model.ImageFile))
                        {
                            path = await _imageHelper.UploadImageAsync(model.ImageFile, "Clients");
                        }
                        else
                        {
                            this.ModelState.AddModelError(string.Empty, "Invalid File. Please upload a File with extension (bmp, gif, png, jpg, jpeg)");
                            return this.View(model);
                        }

                    }

                    if (this.User.IsInRole("Customer"))
                    {
                        var client = _converterHelper.ToClient(model, documentType, path);
                        client.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                        await _clientRepository.UpdateAsync(client);
                    }

                    if (this.User.IsInRole("Doctor"))
                    {
                        var specialty = await _specialtyRepository.GetByIdAsync(model.SpecialtyID);
                        var doctor = _converterHelper.ToDoctor(model, documentType, specialty, path);
                        doctor.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                        await _doctorRepository.UpdateAsync(doctor);
                    }

                    //this.context.Clients.Update(client);

                    //await this.context.SaveChangesAsync();

                    this.ViewBag.UserMessage = "User updated!";
                    //return this.RedirectToAction("Index", "Home");


                    //this.ModelState.AddModelError(string.Empty, "The user couldn't be updated.");

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    //Todo: Fazer melhor tratamento de erro.
                    if (!await _clientRepository.ExistAsync(model.Id) || !await _doctorRepository.ExistAsync(model.Id))
                    {
                        this.ModelState.AddModelError(string.Empty, "User no found.");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, ex.Message);
                    }

                }

            }
            model.Documents = _documentTypeRepository.GetAll();
            return this.View(model);

        }

        // GET:
        public IActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ChangeUser));
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

        public IActionResult RecoverPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }
                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action("ResetPassword", "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Email, "Veterinary Password Reset", $"<h1>Veterinary Password Reset</h1>" +
               $"To reset the password click in this link:</br></br>" +
               $"<a href = \"{link}\">Reset Password</a>");

                this.ViewBag.Message = "The instructions to recover your password has been sent to email.";

                return this.View();
            }

            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);

                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset Successful!";
                    return View();
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
