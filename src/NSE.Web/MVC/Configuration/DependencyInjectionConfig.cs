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

        #region Refit

        /* -------------------------------------------------------------------------- */
        /*                   Utilizando o Refit para consumir API's                   */
        /*                    (https://github.com/reactiveui/refit)                   */
        /* -------------------------------------------------------------------------- */

        // services.AddHttpClient("Refit", options =>
        // {
        //     // ! necessário receber o IConfiguration por parâmetro para acessar a URL do serviço
        //     options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
        // })
        // .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
        // .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);

        #endregion

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IUser, AspNetUser>();
    }
}
