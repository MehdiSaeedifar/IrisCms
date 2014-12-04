using System;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class ForgottenPasswordService : IForgottenPasswordService
    {
        private readonly IDbSet<ForgottenPassword> _forgottenPasswords;
        private readonly IUnitOfWork _uow;

        public ForgottenPasswordService(IUnitOfWork uow)
        {
            _uow = uow;
            _forgottenPasswords = _uow.Set<ForgottenPassword>();
        }

        public void Add(ForgottenPassword model)
        {
            _forgottenPasswords.Add(model);
        }

        public DateTime RequestDate(string key)
        {
            ForgottenPassword selectedItem = _forgottenPasswords.FirstOrDefault(x => x.Key == key);
            if (selectedItem != null)
                return selectedItem.ResetDateTime;
            throw new ArgumentException("This Key is not exist", "key");
        }

        public User FindUser(string key)
        {
            ForgottenPassword selectedItem = _forgottenPasswords.FirstOrDefault(x => x.Key == key);
            if (selectedItem != null)
                return selectedItem.User;
            throw new ArgumentException("This Key is not exist", "key");
        }


        public void Remove(string key)
        {
            _forgottenPasswords.Remove(_forgottenPasswords.FirstOrDefault(x => x.Key == key));
        }
    }
}