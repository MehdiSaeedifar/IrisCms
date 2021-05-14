using System;
using DNTPersianUtils.Core;

namespace Iris.Utilities
{
    public class ConvertToPersian
    {
        public static string ConvertToPersianString(object digit)
        {
            return Convert.ToInt32(digit).ToPersianNumbers();
        }
    }
}