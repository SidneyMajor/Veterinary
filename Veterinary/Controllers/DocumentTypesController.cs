using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinary.Data;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;

namespace Veterinary.Controllers
{
    public class DocumentTypesController : Controller
    {
       
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypesController(IDocumentTypeRepository documentTypeRepository)
        {
           
           _documentTypeRepository = documentTypeRepository;
        }

        // GET: DocumentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _documentTypeRepository.GetAll().ToListAsync());
        }

        // GET: DocumentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _documentTypeRepository.GetByIdAsync(id.Value);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
               await _documentTypeRepository.CreateAsync(documentType);                
                return RedirectToAction(nameof(Index));
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _documentTypeRepository.GetByIdAsync(id.Value);
            if (documentType == null)
            {
                return NotFound();
            }
            return View(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _documentTypeRepository.UpdateAsync(documentType);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _documentTypeRepository.ExistAsync(documentType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(documentType);
        }

        // GET: DocumentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType =  await _documentTypeRepository.GetByIdAsync(id.Value);
            if (documentType == null)
            {
                return NotFound();
            }

            return View(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documentType = await _documentTypeRepository.GetByIdAsync(id);
           await _documentTypeRepository.DeleteAsync(documentType);
           
            return RedirectToAction(nameof(Index));
        }

        
    }
}
