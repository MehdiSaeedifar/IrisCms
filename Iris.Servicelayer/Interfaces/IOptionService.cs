using Iris.Model;
using Iris.Model.AdminModel;

namespace Iris.Servicelayer.Interfaces
{
    public interface IOptionService
    {
        bool ModeratingComment { get; }
        SiteConfig GetAll();
        void Update(UpdateOptionModel model);
    }
}