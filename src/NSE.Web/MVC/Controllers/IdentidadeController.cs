using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class IdentidadeController : MainController
{
    private readonly IAutenticacaoService _autenticacaoService;

    public IdentidadeController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpGet]
    [Route("nova-conta")]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [Route("nova-conta")]
    public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid)
        {
            return View(usuarioRegistro);
        }

        var usuarioResponse = await _autenticacaoService.Registro(usuarioRegistro);

        if (ResponseResultPossuiErros(usuarioResponse.ResponseResult))
        {
            return View(usuarioRegistro);
        }

        await RealizarLogin(usuarioResponse);

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(usuarioLogin);
        }

        var usuarioResponse = await _autenticacaoService.Login(usuarioLogin);

        if (ResponseResultPossuiErros(usuarioResponse.ResponseResult))
        {
            return View(usuarioLogin);
        }

        await RealizarLogin(usuarioResponse);

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    private async Task RealizarLogin(UsuarioRespostaLogin usuarioResponse)
    {
        if (usuarioResponse.AccessToken is null)
        {
            throw new ArgumentNullException(nameof(usuarioResponse.AccessToken));
        }

        var jwtToken = ObterTokenFormatado(usuarioResponse.AccessToken);

        var claims = new List<Claim>();
        claims.Add(new Claim("JWT", usuarioResponse.AccessToken));
        claims.AddRange(jwtToken.Claims);

        var identityClaims = new ClaimsIdentity(
            claims: claims,
            authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
            principal: new ClaimsPrincipal(identityClaims),
            properties: authProperties);
    }

    private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        => new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
}
