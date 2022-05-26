using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class IdentidadeController : Controller
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
    public async Task<IActionResult> Registro(UsuarioRegistro usuario)
    {
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        // API - Registro
        var response = await _autenticacaoService.Registro(usuario);

        /*
        TODO
            => if Fail:
                return View(usuario)

            => Ok: Realizar Login na APP
                return RedirectToAction("Index", "Home");
        */

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
    public async Task<IActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        // API - Login
        var response = await _autenticacaoService.Login(usuario);

        /*
        TODO
            => if Fail:
                return View(usuario)

            => Ok: Realizar Login na APP
                return RedirectToAction("Index", "Home");
        */

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        // Limpar o cookie para deslogar o usuário

        return RedirectToAction(actionName: "Index", controllerName: "Home");
    }
}
