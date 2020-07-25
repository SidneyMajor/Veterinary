using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class DocumentType:IEntity
    {
        public int Id { get; set; }

        public string Document { get; set; }


        public bool WasDeleted { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


        public ICollection<Client> Clients { get; set; }
    }
}
