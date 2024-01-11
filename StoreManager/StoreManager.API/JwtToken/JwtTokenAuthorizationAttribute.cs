using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoreManager.API.JwtToken;

public class JwtTokenAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string token = context.HttpContext.Request.Headers["Authorization"];

        if (!ValidateJwtToken(token))
        {
            context.HttpContext.Response.StatusCode = 401;
            throw new HttpRequestException($"Invalid token: {token}");
        }
    }

    private bool ValidateJwtToken(string token)
    {
        if (TokenBlackList.IsTokenRevoked(token)) return false;

        return true;
    }
}
