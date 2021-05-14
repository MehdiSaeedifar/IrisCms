using System.Threading;

namespace Iris.Utilities
{
    public static class LocalizationHelper
    {
        public static Language GetCurrentLanguage()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name.ToLowerInvariant();

            if (currentCulture == "fa-ir" || currentCulture == "fa")
            {
                return Language.Farsi;
            }

            return Language.English;
        }
    }

    public enum Language
    {
        Farsi,
        English
    }
}
