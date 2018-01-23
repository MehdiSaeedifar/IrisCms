using System;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CaptchaMvc.Infrastructure;
using Iris.Datalayer.Context;
using Iris.Datalayer.Migrations;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.DateAndTime;
using Iris.Web.Binders;
using Iris.Web.DependencyResolution;
using Iris.Web.IrisMembership;
using MvcSiteMapProvider.Web;

namespace Iris.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {

            ModelBinders.Binders.Add(typeof(DateTime?), new PersianDateModelBinder());

            //Database.SetInitializer<IrisDbContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IrisDbContext, Configuration>());

            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());


            XmlSiteMapController.RegisterRoutes(RouteTable.Routes); // <-- register sitemap.xml, add this line of code

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Remove Extra ViewEngins
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;
            CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var principalService = SmObjectFactory.Container.GetInstance<IPrincipalService>();
            // Set the HttpContext's User to our IPrincipal
            Context.User = principalService.GetCurrent();

            if (Context.User == null)
                return;

            if (!Context.User.Identity.IsAuthenticated)
                return;

            var userService = SmObjectFactory.Container.GetInstance<IUserService>();
            var userStatus = userService.GetStatus(Context.User.Identity.Name);
            if (userStatus.IsBaned || !Context.User.IsInRole(userStatus.Role))
            {
                var formsAuthenticationService = SmObjectFactory.Container.GetInstance<IFormsAuthenticationService>();
                formsAuthenticationService.SignOut();
            }

            var dbContext = SmObjectFactory.Container.GetInstance<IUnitOfWork>();
            userService.UpdateUserLastActivity(User.Identity.Name, DateAndTime.GetDateTime());
            dbContext.SaveChanges();
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null || app.Context == null)
                return;
            app.Context.Response.Headers.Remove("Server");
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            SmObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}