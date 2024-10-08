/*
 * Author: Sudesh Sachintha Bandara
 * Description: This class defines the RefreshTokenRequest DTO, which is used to capture
 * the JWT token and refresh token details for requesting a new access token.
 * Date Created: 2024/09/28
 */

namespace ECommerceBackend.DTOs.Request.Auth
{
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Gets or sets the JWT token that is being refreshed.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token associated with the user's session.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
