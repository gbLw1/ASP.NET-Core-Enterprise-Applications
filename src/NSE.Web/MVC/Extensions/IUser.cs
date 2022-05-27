using System.Security.Claims;

namespace MVC.Extensions;

public interface IUser
{
    string Name { get; }
    Guid ObterUserId();
    string ObterUserEmail();
    string ObterUserToken();
    bool EstaAutenticado();
    bool PossuiRole(string role);
    IEnumerable<Claim> ObterClaims();
    HttpContext ObterHttpContext();
}

public class AspNetUser : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => ObterHttpContext().User?.Identity?.Name ?? string.Empty;

    public bool EstaAutenticado()
        => ObterHttpContext().User?.Identity?.IsAuthenticated ?? false;

    public Guid ObterUserId()
        => EstaAutenticado() ? Guid.Parse(ObterHttpContext().User.GetUserId()) : Guid.Empty;

    public string ObterUserEmail()
        => EstaAutenticado() ? ObterHttpContext().User.GetUserEmail() : string.Empty;

    public string ObterUserToken()
        => EstaAutenticado() ? ObterHttpContext().User.GetUserToken() : string.Empty;

    public bool PossuiRole(string role)
        => ObterHttpContext().User.IsInRole(role);

    public IEnumerable<Claim> ObterClaims()
        => ObterHttpContext().User.Claims;

    public HttpContext ObterHttpContext()
        => _accessor?.HttpContext ?? throw new ArgumentNullException("Acessor de contexto HTTP não pode ser nulo.");
}

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("sub");

        return claim?.Value;
    }

    public static string GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("email");

        return claim?.Value;
    }

    public static string GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal is null)
        {
            throw new ArgumentNullException(nameof(principal));
        }

        var claim = principal.FindFirst("JWT");

        return claim?.Value;
    }
}
