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


        /// <summary>
        /// Get Document Type
        /// </summary>
        /// <returns>List Document types</returns>
        public async Task<IEnumerable<DocumentType>> GetComboDocuments()
        {

            return await _context.DocumentTypes.Where(d => d.WasDeleted == false).ToListAsync();

        }

        /// <summary>
        /// Get Document type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>document type</returns>
        public async Task<DocumentType> GetDocumentType(int id)
        {
            return await _context.DocumentTypes.FindAsync(id);
        }



    }
}
