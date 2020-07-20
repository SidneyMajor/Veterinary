using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }

        DateTime CreatedDate { get; set; }


        DateTime UpdateDate { get; set; }


        bool WasDeleted { get; set; }

    }
}
