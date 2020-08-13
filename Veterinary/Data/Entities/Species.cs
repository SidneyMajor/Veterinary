using System;

namespace Veterinary.Data.Entities
{
    public class Species:IEntity
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool WasDeleted { get; set; }

        public string Description { get; set; }
    }
}