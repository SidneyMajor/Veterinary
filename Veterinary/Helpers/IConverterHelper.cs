using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;
using Veterinary.Models;

namespace Veterinary.Helpers
{
   public interface IConverterHelper
    {
        Client ToClient(RegisterNewUserViewModel model, DocumentType documentType, string path);

        //RegisterNewUserViewModel ToRegisterNewUserViewModel(Client model, DocumentType documentType);

        ChangeUserViewModel ToChangeUserViewModel(Client model, DocumentType documentType);

        Client ToClient(ChangeUserViewModel model, DocumentType documentType, string path);

        Animal ToAnimal(AnimalViewModel model, Species species, string path, bool isNew);

        AnimalViewModel ToRegisterNewAnimalViewModel(Animal model, Species species);
    }
}
