using Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.Application.Commands;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Clientes.Controllers;

public class ClientesController : MainController
{
    private readonly IMediatorHandler _mediator;

    public ClientesController(IMediatorHandler mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("clientes")]
    public async Task<IActionResult> Index()
    {
        var resultado = await _mediator.EnviarComando(new RegistrarClienteCommand(Guid.NewGuid(),
                                                            "Gabriel",
                                                            "gabriel@teste.com",
                                                            "85024608038"));

        return CustomResponse(resultado);
    }
}
