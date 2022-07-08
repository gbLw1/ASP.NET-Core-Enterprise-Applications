using Core.Tools;
using NSE.MessageBus;

namespace NSE.Pagamentos.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            //.AddHostedService<PagamentoIntegrationHandler>();
    }
}
