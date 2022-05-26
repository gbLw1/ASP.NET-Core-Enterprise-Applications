using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class IdentidadeController : Controller
{
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
        /*
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
        /*
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
