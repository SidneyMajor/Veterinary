﻿using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
    public interface IConverterHelper
    {
        Client ToClient(RegisterNewUserViewModel model, DocumentType documentType, string path);

        Client ToClient(ChangeUserViewModel model, DocumentType documentType, string path);


        Doctor ToDoctor(RegisterNewDoctorViewModel model, DocumentType documentType, Specialty specialty, string path);

        Doctor ToDoctor(ChangeUserViewModel model, DocumentType documentType, Specialty specialty, string path);


        ChangeUserViewModel ToChangeUserViewModel(Client model);

        ChangeUserViewModel ToChangeUserViewModel(Doctor model);



        Animal ToAnimal(AnimalViewModel model, Species species, string path, bool isNew);

        AnimalViewModel ToAnimalViewModel(Animal model);


        Appointment ToAppointment(AppointmentViewModel model, bool isNew);

        AppointmentViewModel ToAppointmentViewModel(Appointment model);


        DoctorAppointmentViewModel ToDoctorAppointmentViewModel(Appointment model);

        Appointment ToAppointment(DoctorAppointmentViewModel model);
    }
}
