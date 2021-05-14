using Iris.Datalayer.Context;
using Iris.Model;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Caching;
using Iris.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iris.Web.Areas.Admin.Controllers
{
    [SiteAuthorize(Roles = "admin")]
    [Area("Admin")]
    public class OptionController : Controller
    {
        private readonly IOptionService _optionService;
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cacheService;

        public OptionController(IUnitOfWork uow, IOptionService optionService, ICacheService cacheService)
        {
            _uow = uow;
            _optionService = optionService;
            _cacheService = cacheService;
        }

        public virtual ActionResult Index()
        {
            return PartialView("_Index", _optionService.GetAll());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Index(SiteConfig model)
        {
            _optionService.Update(model);
            _uow.SaveChanges();
            _cacheService.RemoveSiteConfig();
            return PartialView("_Alert", new Alert
            {
                Message = "تنظیمات سایت با موفقیت به روز رسانی شد",
                Mode = AlertMode.Success
            });
        }
    }
}