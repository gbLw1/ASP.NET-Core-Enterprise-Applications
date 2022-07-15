using Core.Messages.Integration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NSE.Identidade.API.Services;
using NSE.Identidade.Models;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Identidade.Controllers;

[Route("api/identidade")]
public class AuthController : MainController
{
    private readonly IMessageBus _bus;
    private readonly AuthenticationService _authenticationService;

    public AuthController(IMessageBus bus, AuthenticationService authenticationService)
    {
        _bus = bus;
        _authenticationService = authenticationService;
    }

    [HttpPost("nova-conta")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuario)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        var user = new IdentityUser
        {
            UserName = usuario.Email,
            Email = usuario.Email,
            EmailConfirmed = true
        };

        // tenta criar o usuário
        var result = await _authenticationService.UserManager.CreateAsync(user, usuario.Senha);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                AdicionarErroProcessamento(error.Description);
            }

            return CustomResponse();
        }

        await _authenticationService.SignInManager.SignInAsync(user: user, isPersistent: false);

        // tenta criar o cliente
        var clienteResult = await RegistrarCliente(usuario);

        // se não conseguir criar o cliente, retorna erro e deleta o usuário
        if (!clienteResult.ValidationResult.IsValid)
        {
            await _authenticationService.UserManager.DeleteAsync(user);
            return CustomResponse(clienteResult.ValidationResult);
        }

        return CustomResponse(await _authenticationService.GerarJwt(usuario.Email!));
    }

    [HttpPost("autenticar")]
    public async Task<ActionResult> Login(UsuarioLogin usuario)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponse(ModelState);
        }

        var result = await _authenticationService.SignInManager.PasswordSignInAsync(
            userName: usuario.Email,
            password: usuario.Senha,
            isPersistent: false,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return CustomResponse(await _authenticationService.GerarJwt(usuario.Email!));
        }

        if (result.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas.");
            return CustomResponse();
        }

        AdicionarErroProcessamento("Usuário ou Senha incorretos.");
        return CustomResponse();
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        var usuario = await _authenticationService.UserManager.FindByEmailAsync(usuarioRegistro.Email);

        var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(
            Guid.Parse(usuario.Id),
            usuarioRegistro.Nome!,
            usuarioRegistro.Email!,
            usuarioRegistro.Cpf!);

        try
        {
            return await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch (Exception ex)
        {
            await _authenticationService.UserManager.DeleteAsync(usuario);
            throw;
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            AdicionarErroProcessamento("Refresh token inválido");
            return CustomResponse();
        }

        var token = await _authenticationService.ObterRefreshToken(Guid.Parse(refreshToken));

        if (token is null)
        {
            AdicionarErroProcessamento("Refresh token expirado");
            return CustomResponse();
        }

        return CustomResponse(await _authenticationService.GerarJwt(token.UserName!));
    }
}
