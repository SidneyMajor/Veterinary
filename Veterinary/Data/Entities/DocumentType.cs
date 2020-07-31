﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Veterinary.Data.Entities
{
    public class DocumentType:IEntity
    {
        public int Id { get; set; }
        
        
        public string Description { get; set; }


        [Display(Name = "Is Inactive?")]
        public bool WasDeleted { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }

        
    }
}
