using System.Security.Cryptography;
using System.Text;

namespace SocialMediaApi.Models.Helpers
{
    public static class StringHasher
    {
        public static string HashString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = algorithm.ComputeHash(textBytes);

                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }
    }
}
