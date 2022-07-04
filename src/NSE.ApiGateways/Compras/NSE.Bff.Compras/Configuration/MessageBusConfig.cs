namespace NSE.Bff.Compras.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
    }
}
