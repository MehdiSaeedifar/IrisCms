using System.Web.Mvc;

namespace Iris.Web.Controllers
{
    public partial class ErrorController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult NotFound()
        {
            return View();
        }
    }
}