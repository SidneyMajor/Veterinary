using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IDocumentTypeRepository : IGenericRepository<DocumentType>
    {
        Task<IEnumerable<DocumentType>> GetComboDocuments();

        Task<DocumentType> GetDocumentType(int id);
    }
}
