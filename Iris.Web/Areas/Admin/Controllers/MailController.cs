using System.Linq;
using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.Model.AdminModel;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Mail;
using Iris.Web.Filters;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    public partial class MailController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public MailController(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
        }

        public virtual ActionResult Index()
        {
            return PartialView(MVC.Admin.Mail.Views._Index);
        }

        [HttpGet]
        public virtual ActionResult Send()
        {
            return PartialView(MVC.Admin.Mail.Views._Send);
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Send(SendingMailModel mailModel)
        {
            EMail.EnableSsl = false;
            EMail.From = "mehdi@yahoo.com";
            EMail.Password = "";
            EMail.SmtpPort = 25;
            EMail.SmtpServer = "adad.casd";
            EMail.UserName = "";

            //EMail.Send("sadasd@sad.com", "adsadsad", new ViewConvertor().RenderRazorViewToString(MVC.Admin.Mail.Views._Send,
            //this.ControllerContext,null) , ref error);
            //foreach (var to in mailModel.SendTo)
            //{
            //    switch (to)
            //    {
            //        case "all":
            //            foreach (var email in this._userService.GetUsersEmails())
            //            {
            //                EMail.Send(email, mailModel.Subject, mailModel.Body, ref error);
            //                return Content("salam");
            //            }
            //            break;

            //        case "admins":

            //            break;
            //        case "writers":
            //            break;
            //        case "users":
            //            break;
            //        case "editors":
            //            break;
            //        default:
            //            break;
            //    }
            //}

            return Content("salam");
        }

        public virtual ActionResult AutoCompleteSearch(string term)
        {
            var data = _userService.SearchUser(term).Select(user => new { value = user.Id, text = user.UserName });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}