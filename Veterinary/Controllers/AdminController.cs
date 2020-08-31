using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinary.Data.Repository;
using Veterinary.Helpers;

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

        public AdminController(IDocumentTypeRepository documentTypeRepository,
            IUserHelper userHelper, IClientRepository clientRepository,
            //IImageHelper imageHelper, IConverterHelper converterHelper,
            /*IMailHelper mailHelper,*/ ISpecialtyRepository specialtyRepository,
            IDoctorRepository doctorRepository)
        {
            _documentTypeRepository = documentTypeRepository;
            _clientRepository = clientRepository; 
            _userHelper = userHelper;
            //_imageHelper = imageHelper;
            //_converterHelper = converterHelper;
            //_mailHelper = mailHelper;
            _specialtyRepository = specialtyRepository;
            _doctorRepository = doctorRepository;
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
        [Authorize(Roles = "Admin")]
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

    }
}
