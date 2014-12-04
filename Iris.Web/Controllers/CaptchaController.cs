using System.Web.Mvc;
using CaptchaMvc.Attributes;

namespace Iris.Web.Controllers
{
    public partial class CaptchaController : Controller
    {
        //
        // GET: /Captcha/


        public virtual ActionResult Index(string m)
        {
            return View();
        }

        [HttpPost]
        [CaptchaVerify("کلمه وارد شده صحیح نمی باشد")]
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