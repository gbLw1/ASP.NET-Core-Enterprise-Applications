using Core.Tools;
using NSE.Catalogo.API.Services;
using NSE.MessageBus;

namespace NSE.Catalogo.Configurations;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<CatalogoIntegrationHandler>();
    }
}