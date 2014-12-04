using Persia;

namespace Iris.Utilities
{
    public class ConvertToPersian
    {
        public static string ConvertToPersianString(object digit)
        {
            return PersianWord.ToPersianString(digit);
        }
    }
}