using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Usuario;

public class AspNetUser : IAspNetUser
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

    public string ObterUserRefreshToken()
        => EstaAutenticado() ? ObterHttpContext().User.GetUserRefreshToken() : string.Empty;

    public bool PossuiRole(string role)
        => ObterHttpContext().User.IsInRole(role);

    public IEnumerable<Claim> ObterClaims()
        => ObterHttpContext().User.Claims;

    public HttpContext ObterHttpContext()
        => _accessor?.HttpContext ?? throw new ArgumentNullException("Acessor de contexto HTTP não pode ser nulo.");
}
