using Iris.DomainClasses.Entities;

namespace Iris.Servicelayer.Interfaces
{
    public interface IAnonymousUser
    {
        void Add(AnonymousUser user);
        AnonymousUser GetUser(string name);
        AnonymousUser GetUser(int id);
        AnonymousUser GetUser(string name, string ip);
    }
}
