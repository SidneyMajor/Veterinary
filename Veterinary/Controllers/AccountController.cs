﻿using System;
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
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper,
            IClientRepository clientRepository, 
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _documentTypeRepository = documentTypeRepository;
           _userHelper = userHelper;
           _clientRepository = clientRepository;
           _imageHelper = imageHelper;
           _converterHelper = converterHelper;
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Documents = _documentTypeRepository.GetAll().ToList(),
            };
            
            return View(model);
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (model.ImageFile!=null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Clients");
                }

                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user==null)
                {
                    user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                    };

                    var documentType = await _documentTypeRepository.GetDocumentType(model.DocumentTypeID);
                    //var client = new Client
                    //{
                    //    FirstName=model.FirstName,
                    //    LastName=model.LastName,
                    //    Address=model.Address,
                    //    ZipCode=model.ZipCode,
                    //    PhoneNumber=model.PhoneNumber,
                    //    TaxNumber=model.TaxNumber,
                    //    Gender=model.Gender,
                    //    DateOfBirth=model.DateOfBirth.Value.Date,
                    //    Nationality=model.Nationality,
                    //    DocumentTypeID=model.DocumentTypeID,
                    //    DocumentType=documentType,
                    //    Document=model.Document,
                    //    User=user,                       
                    //};

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result!=IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        //model.Documents = _documentTypeRepository.GetAll().ToList();
                        return this.View(model);
                    }
                    await _userHelper.AddUserToRoleAsync(user, "Owner");
                    var client = _converterHelper.ToClient(model, documentType, path);
                    client.User = user;
                    await _clientRepository.CreateAsync(client);

                    return this.RedirectToAction("Login", "Account");
                }
                this.ModelState.AddModelError(string.Empty, "The user already exists.");
            }
            
            //model.Documents = _documentTypeRepository.GetAll().ToList();
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

        // GET: Clients
        public async Task<IActionResult> ChangeUser()
        {
            //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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

            var documentType = await _documentTypeRepository.GetByIdAsync(client.DocumentTypeID);            
            var model = _converterHelper.ToChangeUserViewModel(client,documentType);
            model.Documents = _documentTypeRepository.GetAll();
            return this.View(model);
          
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {

            if (this.ModelState.IsValid)
            {
                //var client = await _clientRepository.GetClientByUserEmailAsync(this.User.Identity.Name);

                
                    var documentType = await _documentTypeRepository.GetByIdAsync(model.DocumentTypeID);


                    var path = string.Empty;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Clients");
                    }

                    //client.FirstName = model.FirstName;
                    //client.LastName = model.LastName;
                    //client.Address = model.Address;
                    //client.ZipCode = model.ZipCode;
                    //client.PhoneNumber = model.PhoneNumber;
                    //client.TaxNumber = model.TaxNumber;
                    //client.Gender = model.Gender;
                    //client.DateOfBirth = model.SelectDate.Value;
                    //client.Nationality = model.Nationality;
                    //client.Document = model.Document;
                    //client.DocumentTypeID = model.DocumentTypeID;
                    //client.DocumentType = documentType;

                    var client = _converterHelper.ToClient(model, documentType, path);

                    try
                    {
                        await _clientRepository.UpdateAsync(client);

                        //this.context.Clients.Update(client);

                        //await this.context.SaveChangesAsync();

                        this.ViewBag.UserMessage = "User updated!";
                        //return this.RedirectToAction("Index", "Home");


                        //this.ModelState.AddModelError(string.Empty, "The user couldn't be updated.");

                    }
                    catch (Exception ex)
                    {

                        this.ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }               
               

            }            
           // model.Documents = _documentTypeRepository.GetAll();
            return this.View(model);

        }


    }
}
