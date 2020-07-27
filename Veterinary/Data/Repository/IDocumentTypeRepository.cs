using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public interface IDocumentTypeRepository: IGenericRepository<DocumentType>
    {
        //IEnumerable<SelectListItem> GetComboDocuments();        
        Task<DocumentType> GetDocumentType(int id);
    }
}
