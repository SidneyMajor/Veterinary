using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Veterinary.Data.Repository;
using Veterinary.Models;

namespace Veterinary.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public AccountController(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public IActionResult Regiter()
        {
            var model = new RegisterNewUserViewModel
            {
                Documents = _documentTypeRepository.GetComboDocuments(),   
            };

            return View(model);
        }
    }
}
