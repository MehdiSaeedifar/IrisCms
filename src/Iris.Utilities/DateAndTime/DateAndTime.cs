using System;
using DNTPersianUtils.Core;

namespace Iris.Utilities.DateAndTime
{
    public class DateAndTime
    {
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public static string ConvertToPersian(DateTime dateTime, string mod = "")
        {
            return dateTime.ToShortPersianDateTimeString();
        }
    }
}