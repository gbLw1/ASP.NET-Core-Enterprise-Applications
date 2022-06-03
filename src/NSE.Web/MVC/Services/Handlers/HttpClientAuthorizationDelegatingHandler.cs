using System.Net.Http.Headers;
using MVC.Extensions;

namespace MVC.Services.Handlers;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IUser _user;

    public HttpClientAuthorizationDelegatingHandler(IUser user)
    {
        _user = user;
    }

    // * Handler que intercepta o request e adiciona o token de acesso no Header *
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
        }

        var token = _user.ObterUserToken();

        if (token is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
