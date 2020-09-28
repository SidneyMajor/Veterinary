using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<DocumentType>> GetComboDocuments()
        {

            return await _context.DocumentTypes.Where(d => d.WasDeleted == false).ToListAsync();

        }

        public async Task<DocumentType> GetDocumentType(int id)
        {
            return await _context.DocumentTypes.FindAsync(id);
        }



    }
}
