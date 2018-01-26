using System;
using System.Threading;
using System.Web;
using Castle.DynamicProxy;
using Iris.Datalayer.Context;
using Iris.Servicelayer.EFServices;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Caching;
using Iris.Web.Caching;
using Iris.Web.Email;
using Iris.Web.Infrastructure;
using Iris.Web.IrisMembership;
using StructureMap;
using StructureMap.Web;
using StructureMap.Web.Pipeline;

namespace Iris.Web.DependencyResolution
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            var container = new Container(ioc =>
            {
                ioc.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                ioc.For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));

                ioc.For<IPrincipalService>().Use<IrisSupportPrincipalService>();
                ioc.For<IFormsAuthenticationService>().Use<FormsAuthenticationService>();

                var dynamicProxy = new ProxyGenerator();

                ioc.For<IUnitOfWork>().HttpContextScoped().Use<IrisDbContext>();
                ioc.For<IUserService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<UserService>();

                ioc.For<IRoleService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<RoleService>();

                ioc.For<IPostService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<PostService>();

                ioc.For<IBookService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<BookService>();

                ioc.For<ILabelService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<LabelService>();

                ioc.For<IDownloadLinkService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<DownloadLinkService>();

                ioc.For<ICommentService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<CommentService>();

                ioc.For<IAnonymousUser>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<AnounymousUserService>();

                ioc.For<IPageService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<PageService>();

                ioc.For<IOptionService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<OptionService>();

                ioc.For<IPageService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<PageService>();

                ioc.For<ICategoryService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<CategoryService>();

                ioc.For<IArticleService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<ArticleService>();

                ioc.For<IForgottenPasswordService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<ForgottenPasswordService>();

                ioc.For<IMessageService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<MessageService>();

                ioc.For<ICacheService>().DecorateAllWith(myTypeInterface =>
                        dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<CacheService>();
                ioc.For<IEmailService>().DecorateAllWith(myTypeInterface =>
                      dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<EmailService>();
                ioc.For<IViewConvertor>().DecorateAllWith(myTypeInterface =>
                     dynamicProxy.CreateInterfaceProxyWithTarget(myTypeInterface, new CacheInterceptor())).Use<ViewConvertor>();

            });
            container.AssertConfigurationIsValid();
            return container;
        }

        public static void ReleaseAndDisposeAllHttpScopedObjects()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}