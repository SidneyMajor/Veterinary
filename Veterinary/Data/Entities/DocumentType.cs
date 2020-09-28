﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Veterinary.Data.Entities
{
    public class DocumentType : IEntity
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }


        [Display(Name = "Is Inactive?")]
        public bool WasDeleted { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime UpdatedDate { get; set; }


    }
}
