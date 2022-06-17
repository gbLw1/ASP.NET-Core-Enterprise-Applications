using Core.Mediator;
using Core.Messages.Integration;
using EasyNetQ;
using FluentValidation.Results;
using NSE.Clientes.Application.Commands;

namespace NSE.Clientes.Services;

public class RegistroClienteIntegrationHandler : BackgroundService
{
    private IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus = RabbitHutch.CreateBus("host=localhost:5672");

        _bus.Rpc.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
            new ResponseMessage(await RegistrarCliente(request)));

        return Task.CompletedTask;
    }

    private async Task<ValidationResult> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
        ValidationResult sucesso;

        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            sucesso = await mediator.EnviarComando(clienteCommand);
        }

        return sucesso;
    }
}
