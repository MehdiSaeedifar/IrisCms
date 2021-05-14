using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Controllers
{
    public class CaptchaController : Controller
    {
        public virtual ActionResult Index(string m)
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                ViewBag.data = "کلمه وارد شده صحیح می باشد";
            }

            return View();
        }
    }
}