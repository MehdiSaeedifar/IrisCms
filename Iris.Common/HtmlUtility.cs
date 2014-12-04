using System.Text.RegularExpressions;

namespace Iris.Utilities
{
    public class HtmlUtility
    {
        public static string RemoveHtmlTags(string text)
        {
            return string.IsNullOrEmpty(text)
                ? string.Empty
                : Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }
    }
}