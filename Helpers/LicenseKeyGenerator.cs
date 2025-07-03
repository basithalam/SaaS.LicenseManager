using System.Security.Cryptography;
using System.Text;

namespace SaaS.LicenseManager.Helpers
{
    public static class LicenseKeyGenerator
    {
        public static string GenerateKey(string email)
        {
            using var sha256 = SHA256.Create();
            var rawData = email + Guid.NewGuid();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes).Substring(0, 25).ToUpper();
        }
    }
}