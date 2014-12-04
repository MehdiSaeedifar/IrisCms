// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Iris.Web.Controllers
{
    public partial class UserController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected UserController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Index()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult LogOn()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOn);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ChangeAvatar()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeAvatar);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ExistsUserByUserName()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExistsUserByUserName);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ExistsUserByEmail()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExistsUserByEmail);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public UserController Actions { get { return MVC.User; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "User";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "User";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string ProfilePage = "ProfilePage";
            public readonly string UserDetail = "UserDetail";
            public readonly string LogOn = "LogOn";
            public readonly string UpdateProfile = "UpdateProfile";
            public readonly string RemoveAvatar = "RemoveAvatar";
            public readonly string ChangeAvatar = "ChangeAvatar";
            public readonly string Register = "Register";
            public readonly string ExistsUserByUserName = "ExistsUserByUserName";
            public readonly string ExistsUserByEmail = "ExistsUserByEmail";
            public readonly string LogOut = "LogOut";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string ProfilePage = "ProfilePage";
            public const string UserDetail = "UserDetail";
            public const string LogOn = "LogOn";
            public const string UpdateProfile = "UpdateProfile";
            public const string RemoveAvatar = "RemoveAvatar";
            public const string ChangeAvatar = "ChangeAvatar";
            public const string Register = "Register";
            public const string ExistsUserByUserName = "ExistsUserByUserName";
            public const string ExistsUserByEmail = "ExistsUserByEmail";
            public const string LogOut = "LogOut";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string userName = "userName";
        }
        static readonly ActionParamsClass_LogOn s_params_LogOn = new ActionParamsClass_LogOn();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_LogOn LogOnParams { get { return s_params_LogOn; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_LogOn
        {
            public readonly string returnUrl = "returnUrl";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_UpdateProfile s_params_UpdateProfile = new ActionParamsClass_UpdateProfile();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UpdateProfile UpdateProfileParams { get { return s_params_UpdateProfile; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UpdateProfile
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_ChangeAvatar s_params_ChangeAvatar = new ActionParamsClass_ChangeAvatar();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChangeAvatar ChangeAvatarParams { get { return s_params_ChangeAvatar; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChangeAvatar
        {
            public readonly string avatarFile = "avatarFile";
        }
        static readonly ActionParamsClass_Register s_params_Register = new ActionParamsClass_Register();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Register RegisterParams { get { return s_params_Register; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Register
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_ExistsUserByUserName s_params_ExistsUserByUserName = new ActionParamsClass_ExistsUserByUserName();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ExistsUserByUserName ExistsUserByUserNameParams { get { return s_params_ExistsUserByUserName; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ExistsUserByUserName
        {
            public readonly string userName = "userName";
        }
        static readonly ActionParamsClass_ExistsUserByEmail s_params_ExistsUserByEmail = new ActionParamsClass_ExistsUserByEmail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ExistsUserByEmail ExistsUserByEmailParams { get { return s_params_ExistsUserByEmail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ExistsUserByEmail
        {
            public readonly string email = "email";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _EditProfile = "_EditProfile";
                public readonly string _LogOn = "_LogOn";
                public readonly string _Register = "_Register";
                public readonly string _UserDetail = "_UserDetail";
                public readonly string Index = "Index";
                public readonly string LogOn = "LogOn";
                public readonly string ProfilePage = "ProfilePage";
                public readonly string Register = "Register";
            }
            public readonly string _EditProfile = "~/Views/User/_EditProfile.cshtml";
            public readonly string _LogOn = "~/Views/User/_LogOn.cshtml";
            public readonly string _Register = "~/Views/User/_Register.cshtml";
            public readonly string _UserDetail = "~/Views/User/_UserDetail.cshtml";
            public readonly string Index = "~/Views/User/Index.cshtml";
            public readonly string LogOn = "~/Views/User/LogOn.cshtml";
            public readonly string ProfilePage = "~/Views/User/ProfilePage.cshtml";
            public readonly string Register = "~/Views/User/Register.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_UserController : Iris.Web.Controllers.UserController
    {
        public T4MVC_UserController() : base(Dummy.Instance) { }

        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string userName);

        public override System.Web.Mvc.ActionResult Index(string userName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userName", userName);
            IndexOverride(callInfo, userName);
            return callInfo;
        }

        partial void ProfilePageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult ProfilePage()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ProfilePage);
            ProfilePageOverride(callInfo);
            return callInfo;
        }

        partial void UserDetailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult UserDetail()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UserDetail);
            UserDetailOverride(callInfo);
            return callInfo;
        }

        partial void LogOnOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string returnUrl);

        public override System.Web.Mvc.ActionResult LogOn(string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOn);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            LogOnOverride(callInfo, returnUrl);
            return callInfo;
        }

        partial void LogOnOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Iris.Model.LogOnModel model, string returnUrl);

        public override System.Web.Mvc.ActionResult LogOn(Iris.Model.LogOnModel model, string returnUrl)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOn);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            LogOnOverride(callInfo, model, returnUrl);
            return callInfo;
        }

        partial void UpdateProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult UpdateProfile()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateProfile);
            UpdateProfileOverride(callInfo);
            return callInfo;
        }

        partial void UpdateProfileOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Iris.Model.EditProfileModel model);

        public override System.Web.Mvc.ActionResult UpdateProfile(Iris.Model.EditProfileModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateProfile);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            UpdateProfileOverride(callInfo, model);
            return callInfo;
        }

        partial void RemoveAvatarOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult RemoveAvatar()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RemoveAvatar);
            RemoveAvatarOverride(callInfo);
            return callInfo;
        }

        partial void ChangeAvatarOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Web.HttpPostedFileBase avatarFile);

        public override System.Web.Mvc.ActionResult ChangeAvatar(System.Web.HttpPostedFileBase avatarFile)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangeAvatar);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "avatarFile", avatarFile);
            ChangeAvatarOverride(callInfo, avatarFile);
            return callInfo;
        }

        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult Register()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            RegisterOverride(callInfo);
            return callInfo;
        }

        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, Iris.Model.RegisterModel model);

        public override System.Web.Mvc.ActionResult Register(Iris.Model.RegisterModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            RegisterOverride(callInfo, model);
            return callInfo;
        }

        partial void ExistsUserByUserNameOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string userName);

        public override System.Web.Mvc.ActionResult ExistsUserByUserName(string userName)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExistsUserByUserName);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userName", userName);
            ExistsUserByUserNameOverride(callInfo, userName);
            return callInfo;
        }

        partial void ExistsUserByEmailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string email);

        public override System.Web.Mvc.ActionResult ExistsUserByEmail(string email)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExistsUserByEmail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "email", email);
            ExistsUserByEmailOverride(callInfo, email);
            return callInfo;
        }

        partial void LogOutOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        public override System.Web.Mvc.ActionResult LogOut()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOut);
            LogOutOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
