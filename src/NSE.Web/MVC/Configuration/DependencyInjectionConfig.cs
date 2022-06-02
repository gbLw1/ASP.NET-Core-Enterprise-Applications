using MVC.Extensions;
using MVC.Services;
using MVC.Services.Handlers;

namespace MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();
    }
}
