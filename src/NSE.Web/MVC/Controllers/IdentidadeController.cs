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
        if (!ModelState.IsValid) return View(usuarioRegistro);

        var resposta = await _autenticacaoService.Registro(usuarioRegistro);

        if (ResponseResultPossuiErros(resposta.ResponseResult)) return View(usuarioRegistro);

        await _autenticacaoService.RealizarLogin(resposta);

        return RedirectToAction(actionName: "Index", controllerName: "Catalogo");
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
        if (!ModelState.IsValid) return View(usuarioLogin);

        var resposta = await _autenticacaoService.Login(usuarioLogin);

        if (ResponseResultPossuiErros(resposta.ResponseResult)) return View(usuarioLogin);

        await _autenticacaoService.RealizarLogin(resposta);

        if (string.IsNullOrWhiteSpace(returnUrl)) return RedirectToAction(actionName: "Index", controllerName: "Catalogo");

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await _autenticacaoService.Logout();
        return RedirectToAction(actionName: "Index", controllerName: "Catalogo");
    }
}
