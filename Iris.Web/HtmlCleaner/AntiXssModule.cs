using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Iris.Web.HtmlCleaner
{
    public class AntiXssModule : IHttpModule
    {
        #region Fields (3)

        private static readonly Regex CleanAllTags = new Regex("<[^>]+>", RegexOptions.Compiled);

        private static readonly IList<string> IgnoreList = new List<string>
        {
            "__EVENTVALIDATION",
            "__LASTFOCUS",
            "__EVENTTARGET",
            "__EVENTARGUMENT",
            "__VIEWSTATE",
            "__SCROLLPOSITIONX",
            "__SCROLLPOSITIONY",
            "__VIEWSTATEENCRYPTED",
            "__ASYNCPOST",
            "pagedata" //custom
        };

        //اندكي دستكاري در سيستم داخلي دات نت
        private static readonly PropertyInfo ReadonlyProperty = typeof(NameValueCollection).GetProperty("IsReadOnly",
            BindingFlags.Instance | BindingFlags.NonPublic);

        #endregion Fields

        #region Methods (6)

        // Public Methods (2) 

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += cleanUpInput;
        }

        // Private Methods (4) 

        private static void cleanUpAndEncodeCookies(HttpCookieCollection cookiesCollection)
        {
            foreach (string key in cookiesCollection.AllKeys)
            {
                HttpCookie cookie = cookiesCollection[key];
                if (cookie == null) continue;

                foreach (string cookieKey in cookie.Values.AllKeys)
                {
                    string origData = cookie.Values[cookieKey];
                    if (string.IsNullOrEmpty(origData)) continue;
                    origData = origData.Trim();

                    //در حالت كوكي‌ها دليلي براي ارسال هيچ نوع تگي وجود ندارد
                    string modifiedData = HttpUtility.HtmlEncode(CleanAllTags.Replace(origData, string.Empty));
                    if (origData != modifiedData)
                    {
                        //todo: log this attack...
                        cookie.Values[cookieKey] = modifiedData;
                    }
                }
            }
        }

        private static void cleanUpAndEncodeFormFields(NameValueCollection formFieldsCollection)
        {
            ReadonlyProperty.SetValue(formFieldsCollection, false, null); //IsReadOnly=false

            foreach (string key in formFieldsCollection.AllKeys)
            {
                string origData = formFieldsCollection[key];
                if (string.IsNullOrEmpty(origData)) continue;
                origData = origData.Trim();

                //قصد تميز سازي يك سري از موارد را نداريم چون در اين حالت وب فرم‌ها از كار مي‌افتند
                if (IgnoreList.Contains(key)) continue;
                //در ساير موارد كاربران مجازند فقط تگ‌هاي سالم را ارسال كنند و مابقي حذف مي‌شود
                string modifiedData = origData.ToSafeHtml();
                if (origData != modifiedData)
                {
                    //todo: log this attack...                                      
                    formFieldsCollection[key] = modifiedData;
                }
            }

            ReadonlyProperty.SetValue(formFieldsCollection, true, null); //IsReadOnly=true
        }

        private static void cleanUpAndEncodeQueryStrings(NameValueCollection queryStringsCollection)
        {
            ReadonlyProperty.SetValue(queryStringsCollection, false, null); //IsReadOnly=false

            foreach (string key in queryStringsCollection.AllKeys)
            {
                string origData = queryStringsCollection[key];
                if (string.IsNullOrEmpty(origData)) continue;
                origData = origData.Trim();

                //در حالت كوئري استرينگ دليلي براي ارسال هيچ نوع تگي وجود ندارد
                string modifiedData = HttpUtility.HtmlEncode(CleanAllTags.Replace(origData, string.Empty));
                if (origData != modifiedData)
                {
                    //todo: log this attack...
                    queryStringsCollection[key] = modifiedData;
                }
            }

            ReadonlyProperty.SetValue(queryStringsCollection, true, null); //IsReadOnly=true
        }

        private static void cleanUpInput(object sender, EventArgs e)
        {
            HttpRequest request = ((HttpApplication)sender).Request;

            cleanUpAndEncodeQueryStrings(request.QueryString);

            if (request.HttpMethod == "POST")
            {
                cleanUpAndEncodeFormFields(request.Form);
            }

            cleanUpAndEncodeCookies(request.Cookies);
        }

        #endregion Methods
    }
}