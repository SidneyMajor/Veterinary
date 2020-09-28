using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Data.Repository;

namespace Veterinary.Controllers
{
    [Authorize(Roles = "Admin")]
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
            //ViewBag.Document = await _documentTypeRepository.GetAll().ToListAsync();
            return View(await _documentTypeRepository.GetAll().ToListAsync());
        }



        // GET: DocumentTypes/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var model = new DocumentType();
                return PartialView("_AddOrEditPartial", model);
            }
            else
            {

                var documentType = await _documentTypeRepository.GetByIdAsync(id);
                if (documentType == null)
                {
                    return Json(new
                    {
                        isValid = "error",
                        mensage = "Document Type Not Found!"
                    });
                }
                return PartialView("_AddOrEditPartial", documentType);
            }


        }



        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, DocumentType documentType)
        {

            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    try
                    {
                        await _documentTypeRepository.CreateAsync(documentType);
                        //return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException.Message.Contains("duplicate"))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "Theres is already a document type with that description!",
                                model = documentType
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = ex.InnerException.Message,
                                model = documentType
                            });
                        }
                    }

                }
                else
                {
                    try
                    {
                        await _documentTypeRepository.UpdateAsync(documentType);

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await _documentTypeRepository.ExistAsync(documentType.Id))
                        {
                            return Json(new
                            {
                                isValid = "failed",
                                mensage = "There is not exist that document type",
                                model = documentType
                            });
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                var updocumenttype = await _documentTypeRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    documenttypes = Newtonsoft.Json.JsonConvert.SerializeObject(updocumenttype)
                });
            }

            return PartialView("_AddOrEditPartial", documentType);
        }



        // POST: DocumentTypes/Delete/5       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Document Type Not Found!"
                });
            }

            var documentType = await _documentTypeRepository.GetByIdAsync(id.Value);
            if (documentType == null)
            {
                return Json(new
                {
                    isValid = "error",
                    mensage = "Document Type Not Found!"
                });
            }

            try
            {
                await _documentTypeRepository.DeleteAsync(documentType);
                var updocumenttype = await _documentTypeRepository.GetAll().ToListAsync();
                return Json(new
                {
                    isValid = "success",
                    documenttypes = Newtonsoft.Json.JsonConvert.SerializeObject(updocumenttype)
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _documentTypeRepository.ExistAsync(documentType.Id))
                {
                    return Json(new
                    {
                        isValid = "failed",
                        mensage = "There is not exist that document type"
                    });
                }
                else
                {
                    throw;
                }
            }
        }


    }
}
