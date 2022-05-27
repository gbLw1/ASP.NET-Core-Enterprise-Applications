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

        // API - Registro
        var usuarioResponse = await _autenticacaoService.Registro(usuarioRegistro);

        // Falha Registro
        if (ResponsePossuiErros(usuarioResponse.ResponseResult))
        {
            return View(usuarioRegistro);
        }

        // Realizar Login na APP
        await RealizarLogin(usuarioResponse);

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid)
        {
            return View(usuarioLogin);
        }

        // API - Login
        var usuarioResponse = await _autenticacaoService.Login(usuarioLogin);

        // Falha Login
        if (ResponsePossuiErros(usuarioResponse.ResponseResult))
        {
            return View(usuarioLogin);
        }

        // Realizar Login na APP
        await RealizarLogin(usuarioResponse);

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        // Limpar o cookie para deslogar o usuário

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    private async Task RealizarLogin(UsuarioRespostaLogin usuarioResponse)
    {
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
