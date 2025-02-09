﻿using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Animal ToAnimal(AnimalViewModel model, Species species, string path, bool isNew)
        {
            return new Animal
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                Breed = model.Breed,
                Gender = model.Gender,
                Color = model.Color,
                Weight = model.Weight,
                Size = model.Size,
                Remarks = model.Remarks,
                DateOfBirth = model.SelectDate.Value,
                SpeciesID = model.SpeciesID,
                Species = species,
                ImageUrl = path,
                CreatedDate = model.CreatedDate,
            };
        }

        public AnimalViewModel ToAnimalViewModel(Animal model)
        {
            return new AnimalViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Breed = model.Breed,
                Gender = model.Gender,
                Color = model.Color,
                Weight = model.Weight,
                Size = model.Size,
                Remarks = model.Remarks,
                SelectDate = model.DateOfBirth,
                SpeciesID = model.SpeciesID,
                ImageUrl = model.ImageUrl,
                CreatedDate = model.CreatedDate,
            };
        }

        public ChangeUserViewModel ToChangeUserViewModel(Client model)
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
                SelectDate = model.DateOfBirth,
                Nationality = model.Nationality,
                DocumentTypeID = model.DocumentTypeID,
                Document = model.Document,
                ImageUrl = model.ImageUrl,
                CreatedDate = model.CreatedDate,
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
                Id = model.Id,
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
                CreatedDate = model.CreatedDate,
            };
        }

        public Doctor ToDoctor(RegisterNewDoctorViewModel model, DocumentType documentType, Specialty specialty, string path)
        {
            return new Doctor
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
                SpecialtyID = model.SpecialtyID,
                Specialty = specialty,
                SSNumber = model.SSNumber,
            };
        }


        //Todo: completar a conversão para changeuser para o medico
        public ChangeUserViewModel ToChangeUserViewModel(Doctor model)
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
                SelectDate = model.DateOfBirth,
                Nationality = model.Nationality,
                DocumentTypeID = model.DocumentTypeID,
                SpecialtyID = model.SpecialtyID,
                Document = model.Document,
                ImageUrl = model.ImageUrl,
                CreatedDate = model.CreatedDate,
                SSNumber = model.SSNumber,
            };
        }

        public Doctor ToDoctor(ChangeUserViewModel model, DocumentType documentType, Specialty specialty, string path)
        {
            return new Doctor
            {
                Id = model.Id,
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
                SpecialtyID = model.SpecialtyID,
                Specialty = specialty,
                SSNumber = model.SSNumber,
            };
        }

        public Appointment ToAppointment(AppointmentViewModel model, bool isNew)
        {
            return new Appointment
            {
                //Todo: get value to subject 
                Id = isNew ? 0 : model.Id,
                //AppointmentDate = model.SelectDate.Value,
                //AppointmentTime = model.SelectTime.Value,
                //Subject=model.Animal.Name,
                AnimalID = model.AnimalID,
                DoctorID = model.DoctorID,
                SpecialtyID = model.SpecialtyID,
                Status = isNew ? "Pending" : "Accepted",
                StartTime = model.StartTime,
                EndTime = model.StartTime.AddMinutes(30),
            };
        }

        public AppointmentViewModel ToAppointmentViewModel(Appointment model)
        {
            return new AppointmentViewModel
            {
                AnimalID = model.AnimalID,
                DoctorID = model.DoctorID,
                SpecialtyID = model.SpecialtyID,
                Status = model.Status,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
            };
        }

        public DoctorAppointmentViewModel ToDoctorAppointmentViewModel(Appointment model)
        {
            return new DoctorAppointmentViewModel
            {
                Id = model.Id,
                AnimalID = model.AnimalID,
                DoctorID = model.DoctorID,
                SpecialtyID = model.SpecialtyID,
                Status = model.Status,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Remarks = model.Remarks
            };
        }

        public Appointment ToAppointment(DoctorAppointmentViewModel model)
        {
            return new Appointment
            {

                Id = model.Id,
                //AppointmentDate = model.SelectDate.Value,
                //AppointmentTime = model.SelectTime.Value,                
                AnimalID = model.AnimalID,
                DoctorID = model.DoctorID,
                SpecialtyID = model.SpecialtyID,
                Status = "Concluded",
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Remarks = model.Remarks
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
