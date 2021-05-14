using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Servicelayer.Interfaces
{
    public interface IMessageService
    {
        void Add(Message model);
        IList<Message> GetAll();
        Message Find(int id);
    }
}