using System.Web.Mvc;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Web.HtmlCleaner;

namespace Iris.Web.Controllers
{
    public partial class ContactUsController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ContactUsController(IUnitOfWork uow, IMessageService messageService, IUserService userService)
        {
            _uow = uow;
            _messageService = messageService;
            _userService = userService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public virtual ActionResult Submit(ContactUsModel model)
        {
            if (!ModelState.IsValid)
                return PartialView(MVC.Shared.Views._ValidationSummery, model);

            _messageService.Add(new Message
            {
                AddedDate = DateAndTime.GetDateTime(),
                Body = model.Body.ToSafeHtml(),
                Subject = model.Subject,
                IsAnswared = false,
                User = _userService.Find(User.Identity.Name)
            });
            _uow.SaveChanges();

            return PartialView(MVC.Shared.Views._Alert,
                new Alert { Mode = AlertMode.Success, Message = "پیغام شما با موفقیت برای ما ارسال شد." });
        }
    }
}