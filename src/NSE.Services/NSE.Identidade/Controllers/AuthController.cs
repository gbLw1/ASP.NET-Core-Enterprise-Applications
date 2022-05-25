using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identidade.Models;

namespace NSE.Identidade.Controllers;

[ApiController]
[Route("api/identidade")]
public class AuthController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var user = new IdentityUser
        {
            UserName = usuario.Email,
            Email = usuario.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, usuario.Senha);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        await _signInManager.SignInAsync(user: user, isPersistent: false);
        return Ok();
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var result = await _signInManager.PasswordSignInAsync(
            userName: usuario.Email,
            password: usuario.Senha,
            isPersistent: false,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }
}
