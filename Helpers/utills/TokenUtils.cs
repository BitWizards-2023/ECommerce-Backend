/*
 * File: TokenUtils.cs
 * Description: Provides utility methods for generating secure refresh and reset tokens.
 * Author: Sudesh Sachintha Bandara
 * Date: 2024-09-29
 */

using System.Security.Cryptography;

namespace ECommerceBackend.Helpers.utills
{
    public class TokenUtils
    {
        /// <summary>
        /// Generates a secure refresh token.
        /// </summary>
        /// <returns>A base64-encoded secure refresh token.</returns>
        // Generates a secure refresh token using a cryptographically strong random number generator.
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Generates a secure reset token.
        /// </summary>
        /// <returns>A base64-encoded secure reset token.</returns>
        // Generates a secure reset token using a cryptographically strong random number generator.
        public static string GenerateResetToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
