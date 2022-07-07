using Core.Tools;
using NSE.Carrinho.Services;
using NSE.MessageBus;

namespace NSE.Carrinho.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
            .AddHostedService<CarrinhoIntegrationHandler>();
    }
}
