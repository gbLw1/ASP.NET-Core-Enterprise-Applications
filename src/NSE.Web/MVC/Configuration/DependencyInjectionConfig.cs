using Microsoft.AspNetCore.Mvc.DataAnnotations;
using MVC.Extensions;
using MVC.Services;
using MVC.Services.Handlers;
using NSE.WebAPI.Core.Usuario;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace MVC.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddScoped<IAspNetUser, AspNetUser>();

        #region HttpServices

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
            .AddPolicyHandler(PollyExtension.EsperarTentar())
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddHttpClient<ICatalogoService, CatalogoService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            // .AddTransientHttpErrorPolicy(p =>
            //     p.WaitAndRetryAsync(retryCount: 3,
            //                         sleepDurationProvider: _ => TimeSpan.FromMilliseconds(600)));
            .AddPolicyHandler(PollyExtension.EsperarTentar())
            .AddTransientHttpErrorPolicy(p =>
                p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        services.AddHttpClient<IComprasBffService, ComprasBffService>()
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(PollyExtension.EsperarTentar())
            .AddTransientHttpErrorPolicy(
                p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        #endregion

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
    }
}

public class PollyExtension
{
    /* ------------------ (https://github.com/App-vNext/Polly) ------------------ */
    public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
    {
        var retry = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }, (outcome, timespan, retryCount, context) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Tentando pela {retryCount} vez!");
                Console.ForegroundColor = ConsoleColor.White;
            });

        return retry;
    }
}