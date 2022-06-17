using Core.Tools;
using NSE.Clientes.Services;
using NSE.MessageBus;

namespace NSE.Clientes.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<RegistroClienteIntegrationHandler>();
    }
}
