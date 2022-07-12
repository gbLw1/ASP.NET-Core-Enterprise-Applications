using Core.Comunication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using MVC.Extensions;
using NSE.WebAPI.Core.Usuario;
using NSE.WebApp.MVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVC.Services;

public interface IAutenticacaoService
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario);
    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario);
    Task RealizarLogin(UsuarioRespostaLogin resposta);
    Task Logout();
    bool TokenExpirado();
    Task<bool> RefreshTokenValido();
}

public class AutenticacaoService : Service, IAutenticacaoService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authenticationService;
    private readonly IAspNetUser _user;

    public AutenticacaoService(
        HttpClient httpClient,
        IOptions<AppSettings> settings,
        IAuthenticationService authenticationService,
        IAspNetUser user)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl
            ?? throw new ArgumentNullException(nameof(settings.Value.AutenticacaoUrl)));
        _authenticationService = authenticationService;
        _user = user;
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario)
    {
        var loginContent = ObterConteudo(usuario);

        var response = await _httpClient.PostAsync(
            requestUri: "/api/identidade/autenticar",
            content: loginContent);

        if (HttpResponseHasErrors(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario)
    {
        var registroContent = ObterConteudo(usuario);

        var response = await _httpClient.PostAsync(
            requestUri: "/api/identidade/nova-conta",
            content: registroContent);

        if (HttpResponseHasErrors(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin> UtilizarRefreshToken(string refreshToken)
    {
        var refreshTokenContent = ObterConteudo(refreshToken);

        var response = await _httpClient.PostAsync("/api/identidade/refresh-token", refreshTokenContent);

        if (HttpResponseHasErrors(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task RealizarLogin(UsuarioRespostaLogin resposta)
    {
        var token = ObterTokenFormatado(resposta.AccessToken!);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", resposta.AccessToken!));
        claims.Add(new Claim("RefreshToken", resposta.RefreshToken!));
        claims.AddRange(token!.Claims);

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            IsPersistent = true
        };

        await _authenticationService.SignInAsync(
            _user.ObterHttpContext(),
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task Logout()
    {
        await _authenticationService.SignOutAsync(
            _user.ObterHttpContext(),
            CookieAuthenticationDefaults.AuthenticationScheme,
            null);
    }

    public static JwtSecurityToken? ObterTokenFormatado(string jwtToken)
    {
        return new JwtSecurityTokenHandler().ReadToken(jwtToken) as JwtSecurityToken;
    }

    public bool TokenExpirado()
    {
        var jwt = _user.ObterUserToken();
        if (jwt is null) return false;

        var token = ObterTokenFormatado(jwt);
        return token!.ValidTo.ToLocalTime() < DateTime.Now;
    }

    public async Task<bool> RefreshTokenValido()
    {
        var resposta = await UtilizarRefreshToken(_user.ObterUserRefreshToken());

        if (resposta.AccessToken is not null && resposta.ResponseResult is null)
        {
            await RealizarLogin(resposta);
            return true;
        }

        return false;
    }
}
