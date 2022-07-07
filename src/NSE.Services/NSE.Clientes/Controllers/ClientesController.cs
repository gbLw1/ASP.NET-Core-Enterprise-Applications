using Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.Application.Commands;
using NSE.Clientes.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.Controllers;

public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediator;
    private readonly IClienteRepository _clienteRepository;
    private readonly IAspNetUser _user;

    public ClientesController(
        IMediatorHandler mediator,
        IClienteRepository clienteRepository,
        IAspNetUser user)
    {
        _mediator = mediator;
        _clienteRepository = clienteRepository;
        _user = user;
    }

    [HttpGet("cliente/endereco")]
    public async Task<IActionResult> ObterEndereco()
    {
        var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());
        return endereco is null ? NotFound() : CustomResponse(endereco);
    }

    [HttpPost("cliente/endereco")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        endereco.ClienteId = _user.ObterUserId();
        return CustomResponse(await _mediator.EnviarComando(endereco));
    }
}
