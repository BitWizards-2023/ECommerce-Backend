/*
 * File: PasswordHasher.cs
 * Description: Provides utility methods for hashing and verifying passwords using SHA256.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024/09/30
 */

using System.Security.Cryptography;
using System.Text;

namespace ECommerceBackend.Utilities
{
    public static class PasswordHasher
    {
        // Hashes the provided password using SHA256 and returns the hashed password.
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Verifies if the provided password matches the hashed password.
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
