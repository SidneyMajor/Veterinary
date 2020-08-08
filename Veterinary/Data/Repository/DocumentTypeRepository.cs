using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class DocumentTypeRepository : GenericRepository<DocumentType>, IDocumentTypeRepository
    {
        private readonly DataContext _context;

        public DocumentTypeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //public IEnumerable<SelectListItem> GetComboDocuments()
        //{
        //    var list = _context.DocumentTypes.Where(d => d.WasDeleted == false).Select(c => new SelectListItem
        //    {
        //        Text = c.Document,
        //        Value = c.Id.ToString()

        //    }).OrderBy(l => l.Text).ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "(Select a document...)",
        //        Value = "0"
        //    });

        //    return list;

        //}

        public async Task<DocumentType> GetDocumentType(int id)
        {
            return await _context.DocumentTypes.FindAsync(id);
        }

        //public List<DocumentType> Getallteste()
        //{
        //    _ = new List<DocumentType>();

        //    List<DocumentType> list = _context.DocumentTypes.ToList();

        //    return list;
        //}

    }
}
