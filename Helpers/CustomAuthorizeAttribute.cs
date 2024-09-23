// Helpers/CustomAuthorizationHelper.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerceBackend.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _role;

        public CustomAuthorizeAttribute(string role)
        {
            _role = role;
        }

        // Implementation of the OnAuthorization method
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if the user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "You are not authenticated. Please log in." });
                return;
            }

            // Check if the user has the required role
            if (!user.IsInRole(_role))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
