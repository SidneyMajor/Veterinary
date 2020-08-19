using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public ChangeUserViewModel ToChangeUserViewModel(Client model, DocumentType documentType)
        {
            return new ChangeUserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                ZipCode = model.ZipCode,
                PhoneNumber = model.PhoneNumber,
                TaxNumber = model.TaxNumber,
                Gender = model.Gender,
                SelectDate=model.DateOfBirth,                
                Nationality = model.Nationality,
                DocumentTypeID = model.DocumentTypeID,
                DocumentType = documentType,
                Document = model.Document,
                ImageUrl = model.ImageUrl,
                CreatedDate=model.CreatedDate,
            };
        }

        public Client ToClient(RegisterNewUserViewModel model, DocumentType documentType, string path)
        {

            return new Client
            {
               
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                ZipCode = model.ZipCode,
                PhoneNumber = model.PhoneNumber,
                TaxNumber = model.TaxNumber,
                Gender = model.Gender,
                DateOfBirth = model.SelectDate.Value,
                Nationality = model.Nationality,
                DocumentTypeID = model.DocumentTypeID,
                DocumentType = documentType,
                Document = model.Document,
                ImageUrl = path,
            };
        }

        public Client ToClient(ChangeUserViewModel model, DocumentType documentType, string path)
        {
            return new Client
            {
                Id =  model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                ZipCode = model.ZipCode,
                PhoneNumber = model.PhoneNumber,
                TaxNumber = model.TaxNumber,
                Gender = model.Gender,
                DateOfBirth = model.SelectDate.Value,
                Nationality = model.Nationality,
                DocumentTypeID = model.DocumentTypeID,
                DocumentType = documentType,
                Document = model.Document,
                ImageUrl = path,
                CreatedDate=model.CreatedDate,
            };
        }

        //public RegisterNewUserViewModel ToRegisterNewUserViewModel(Client model, DocumentType documentType)
        //{
        //    return new RegisterNewUserViewModel
        //    {
        //        Id =  model.Id,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        Address = model.Address,
        //        ZipCode = model.ZipCode,
        //        PhoneNumber = model.PhoneNumber,
        //        TaxNumber = model.TaxNumber,
        //        Gender = model.Gender,
        //        SelectDate = model.DateOfBirth,
        //        Nationality = model.Nationality,
        //        DocumentTypeID = model.DocumentTypeID,
        //        DocumentType = documentType,
        //        Document = model.Document,
        //        ImageUrl = model.ImageUrl,
        //    };
        //}
    }
}
