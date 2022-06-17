using Core.Mediator;
using Core.Messages.Integration;
using FluentValidation.Results;
using NSE.Clientes.Application.Commands;
using NSE.MessageBus;

namespace NSE.Clientes.Services;

public class RegistroClienteIntegrationHandler : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RegistroClienteIntegrationHandler(
        IServiceProvider serviceProvider,
        IMessageBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
            await RegistrarCliente(request));

        return Task.CompletedTask;
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
    {
        var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
        ValidationResult sucesso;

        using (var scope = _serviceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
            sucesso = await mediator.EnviarComando(clienteCommand);
        }

        return new ResponseMessage(sucesso);
    }
}
