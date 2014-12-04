using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Iris.Web.Controllers
{
    public partial class IrisController : Controller
    {
        // GET: Iris
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}