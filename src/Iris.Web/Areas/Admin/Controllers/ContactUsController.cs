using Iris.Servicelayer.Interfaces;
using Iris.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin,moderator")]
    [Area("Admin")]
    public class ContactUsController : Controller
    {
        private readonly IMessageService _messageService;

        public ContactUsController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public virtual ActionResult Index()
        {
            return PartialView("_Index", _messageService.GetAll());
        }

        public virtual ActionResult Detail(int id)
        {
            return PartialView("_Detail", _messageService.Find(id));
        }
    }
}