namespace Iris.Web.Infrastructure
{
    public interface IViewConvertor
    {
        string RenderRazorViewToString(string viewName, object model);
    }
}