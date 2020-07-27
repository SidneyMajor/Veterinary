using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using Veterinary.Data;
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
        private readonly DataContext context;

        public AccountController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper,
            IClientRepository clientRepository, DataContext context)
        {
            _documentTypeRepository = documentTypeRepository;
           _userHelper = userHelper;
           _clientRepository = clientRepository;
            this.context = context;
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Documents = _documentTypeRepository.GetAll().ToList(),
            };

            //ViewBag.Data = _documentTypeRepository.GetAll().ToList();
            ViewBag.Gender = new List<string> { "M", "F" };
            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user==null)
                {
                    user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                    };

                    var document = await _documentTypeRepository.GetDocumentType(model.DocumentTypeID);
                    var client = new Client
                    {
                        FisrtName=model.FisrtName,
                        LastName=model.LastName,
                        Address=model.Address,
                        ZipCode=model.ZipCode,
                        PhoneNumber=model.PhoneNumber,
                        TaxNumber=model.TaxNumber,
                        Gender=model.Gender,
                        DateOfBirth=model.DateOfBirth.Value.Date,
                        Nationality=model.Nationality,
                        DocumentTypeID=model.DocumentTypeID,
                        DocumetType=document,
                        User=user,                       
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result!=IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

                    await _clientRepository.CreateAsync(client);

                    return this.RedirectToAction("Login", "Account");
                }
            }
            this.ModelState.AddModelError(string.Empty, "The user already exists.");
            return this.View(model);
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

        // GET: Clients only for admin
        //TODO: tenho que trabalhar a view de modo apenas mostrar os btns apagar e detalhes. criar tbm uma para mostrar os utilizadores inativos.
        public async Task<IActionResult> ListClient()
        {

            return View(await _clientRepository.GetAll().Include(u => u.User).ToListAsync());
        }

    }
}
