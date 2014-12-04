using System.Web.Mvc;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Filters;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    public partial class ContactUsController : Controller
    {
        private readonly IMessageService _messageService;

        public ContactUsController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public virtual ActionResult Index()
        {
            return PartialView(MVC.Admin.ContactUs.Views._Index, _messageService.GetAll());
        }

        public virtual ActionResult Detail(int id)
        {
            return PartialView(MVC.Admin.ContactUs.Views._Detail, _messageService.Find(id));
        }
    }
}