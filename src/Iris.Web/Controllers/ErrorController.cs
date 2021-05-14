using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class ErrorController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public new ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
