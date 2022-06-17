using Microsoft.Extensions.Configuration;

namespace Core.Tools;

public static class ConfigurationExtensions
{
    public static string GetMessageQueueConnection(this IConfiguration configuration, string name)
    {
        return configuration?.GetSection("MessageQueueConnection")?[name]
            ?? throw new ArgumentException("MessageQueueConnection not found");
    }
}
