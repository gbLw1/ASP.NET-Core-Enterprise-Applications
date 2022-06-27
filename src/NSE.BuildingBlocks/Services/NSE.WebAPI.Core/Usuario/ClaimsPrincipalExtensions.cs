using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("sub");

        return claim?.Value ?? throw new ArgumentNullException(nameof(claim));
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("email");

        return claim?.Value ?? throw new ArgumentNullException(nameof(claim));
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("JWT");

        return claim?.Value ?? throw new ArgumentNullException(nameof(claim));
    }
}
