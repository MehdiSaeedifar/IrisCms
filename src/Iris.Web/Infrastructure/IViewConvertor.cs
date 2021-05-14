using System.Threading.Tasks;

namespace Iris.Web.Infrastructure
{
    public interface IViewConvertor
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
