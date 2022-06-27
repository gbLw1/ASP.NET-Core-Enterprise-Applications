using NSE.Carrinho.Data;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<CarrinhoContext>();
    }
}
