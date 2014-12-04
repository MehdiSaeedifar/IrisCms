using System;
using System.Security.Cryptography;
using System.Text;

namespace Iris.Web.RSS
{
    public static class CryptoUtils
    {
        public static string SHA1(this string data)
        {
            using (var sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }
    }
}