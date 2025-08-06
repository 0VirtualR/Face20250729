using Dm;
using System.Security.Cryptography;
using System.Text;

namespace FaceAPI.Extensions
{
    public static class StringExtension
    {
        public static string GetMD5(string  input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            var hash=MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
