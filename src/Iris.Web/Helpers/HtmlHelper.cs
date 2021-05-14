using System;
using DNTPersianUtils.Core;
using Iris.Utilities.DateAndTime;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Iris.Web.Helpers
{
    public static class ConverterToPersian
    {
        public static HtmlString ConvertToPersianString(this IHtmlHelper htmlHelper, int digit)
        {
            return new(digit.ToPersianNumbers());
        }

        public static HtmlString ConvertToPersianString(this IHtmlHelper htmlHelper, string str)
        {
            return new (str.ToPersianNumbers());
        }

        public static HtmlString ConvertToPersianDateTime(this IHtmlHelper htmlHelper, DateTime dateTime,
            string mode = "")
        {
            return dateTime.Year == 1 ? null : new(DateAndTime.ConvertToPersian(dateTime, mode));
        }

        public static string ConvertBooleanToPersian(this IHtmlHelper htmlHelper, bool? value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }

        public static string ConvertBooleanToPersian(this IHtmlHelper htmlHelper, bool value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }
    }
}