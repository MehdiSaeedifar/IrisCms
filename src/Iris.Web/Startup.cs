using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetCore.Unobtrusive.Ajax;
using DNTCaptcha.Core;
using DNTCommon.Web.Core;
using EFCoreSecondLevelCacheInterceptor;
using Iris.Datalayer.Context;
using Iris.Servicelayer.EFServices;
using Iris.Servicelayer.Interfaces;
using Iris.Web.Caching;
using Iris.Web.Email;
using Iris.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ICacheService = Iris.Web.Caching.ICacheService;

namespace Iris.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IUnitOfWork, IrisDbContext>((serviceProvider, options) =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>())
            );

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddUnobtrusiveAjax();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ILabelService, LabelService>();
            services.AddTransient<IDownloadLinkService, DownloadLinkService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IAnonymousUser, AnounymousUserService>();
            services.AddTransient<IPageService, PageService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IForgottenPasswordService, ForgottenPasswordService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IViewConvertor, ViewConvertor>();
            services.AddTransient<IOptionService, OptionService>();
            services.AddTransient<IBookSearch, BookSearch>();
            services.AddScoped<IAvatarImage, AvatarImage>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/logon";
                    options.AccessDeniedPath = "/user/logon";
                    options.Events.OnValidatePrincipal = async context =>
                    {
                        var userService =
                            context.HttpContext.RequestServices.GetService<IUserService>();

                        if (context.Principal?.Identity?.Name is null) return;

                        var userStatus = userService!.GetStatus(context.Principal.Identity.Name);

                        if (userStatus.IsBaned || !context.Principal.IsInRole(userStatus.Role))
                        {
                            await context.HttpContext.SignOutAsync();
                        }

                        var dbContext = context.HttpContext.RequestServices.GetService<IUnitOfWork>();
                        userService.UpdateUserLastActivity(context.Principal.Identity.Name, DateTime.Now);
                        dbContext.SaveChanges();
                    };
                });

            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("/bundle/jquery.js",
                            "Scripts/jquery-1.9.1.js",
                            "Scripts/jquery-migrate-1.1.1.js",
                            "Scripts/jquery.unobtrusive-ajax.js",
                            "Scripts/load.js"
                        );

                pipeline.AddJavaScriptBundle("/bundle/sitejs.js",
                    "Scripts/myscript.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/jqueryform.js",
                    "Scripts/jquery.form.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/bootstrap.js",
                    "Scripts/bootstrap/bootstrap-rtl.js",
                    "Scripts/noty/jquery.noty.js",
                    "Scripts/noty/layouts/top.js",
                    "Scripts/noty/layouts/topCenter.js",
                    "Scripts/noty/themes/default.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/jqueryui.js",
                    "Scripts/jquery-ui-1.10.1.js",
                    "Scripts/PersianCalender/calendar.js",
                    "Scripts/PersianCalender/jquery.ui.datepicker-cc-fa.js",
                    "Scripts/PersianCalender/jquery.ui.datepicker-cc.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/jqueryuitools.js",
                    "Scripts/PersianCalender/calendar.js",
                    "Scripts/PersianCalender/jquery.ui.datepicker-cc-fa.js",
                    "Scripts/PersianCalender/jquery.ui.datepicker-cc.js",
                    "Scripts/jquery-ui-1.10.2.autocomplete.js",
                    "Scripts/jquery-validator-combined.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/redactor.js",
                    "Scripts/redactor/redactor.js"
                );

                pipeline.AddJavaScriptBundle("/bundle/admin.js",
                    "Scripts/chosen/chosen.jquery.js",
                    "Scripts/adminjs.js"
                );

                pipeline.AddCssBundle("/bundle/sitecss.css",
                    "Content/Style.css"
                );

                pipeline.AddCssBundle("/bundle/bootstrap-css.css",
                    "Content/bootstrap/bootstrap-rtl.css",
                            "Content/bootstrap/responsive-rtl.css"
                );

                pipeline.AddCssBundle("/bundle/autocompleteandanimations.css",
                    "Content/themes/start/jquery-ui-1.10.2.autocomplete.css",
                            "Content/animate.css",
                            "Content/themes/start/jquery.ui.datepicker.css"
                );

                pipeline.AddCssBundle("/bundle/themes/start/css",
                            "Content/themes/start/jquery.ui.core.css",
                            "Content/themes/start/jquery.ui.resizable.css",
                            "Content/themes/start/jquery.ui.selectable.css",
                            "Content/themes/start/jquery.ui.accordion.css",
                            "Content/themes/start/jquery.ui.autocomplete.css",
                            "Content/themes/start/jquery.ui.button.css",
                            "Content/themes/start/jquery.ui.dialog.css",
                            "Content/themes/start/jquery.ui.menu.css",
                            "Content/themes/start/jquery.ui.slider.css",
                            "Content/themes/start/jquery.ui.tabs.css",
                            "Content/themes/start/jquery.ui.datepicker.css",
                            "Content/themes/start/jquery.ui.progressbar.css",
                            "Content/themes/start/jquery.ui.tooltip.css",
                            "Content/themes/start/jquery.ui.theme.css"
                    );
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSingleton(
                HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic)
            );

            services.AddDNTCommonWeb();

            services.AddDNTCaptcha(options =>
            {
                options
                    .UseCookieStorageProvider() // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                                // .UseDistributedSerializationProvider()

                    // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
                    // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
                    .AbsoluteExpiration(minutes: 7)
                    .ShowThousandsSeparators(false)
                    .WithEncryptionKey("aQ2_asdJabzo4_jH")
                    .InputNames( // This is optional. Change it if you don't like the default names.
                        new DNTCaptchaComponent
                        {
                            CaptchaHiddenInputName = "DNTCaptchaText",
                            CaptchaHiddenTokenName = "DNTCaptchaToken",
                            CaptchaInputName = "DNTCaptchaInputText"
                        })
                    .Identifier("dntCaptcha"); // This is optional. Change it if you don't like its default name.

            });

            services.AddEFSecondLevelCache(options =>
                    options.UseMemoryCacheProvider().DisableLogging(true)

            // Please use the `CacheManager.Core` or `EasyCaching.Redis` for the Redis cache provider.
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseWebOptimizer();

            app.UseStaticFiles();

            app.UseUnobtrusiveAjax();



            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    "Admin",
                    "Admin",
                    "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "LabelRoute",
                    pattern: "{controller=Label}/{action=Index}/{id}/{title}/{name}");

                endpoints.MapControllerRoute(
                    name: "UserRoute",
                    pattern: "{controller=User}/{action=Index}/{userName}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{title?}");
            });
        }
    }
}
