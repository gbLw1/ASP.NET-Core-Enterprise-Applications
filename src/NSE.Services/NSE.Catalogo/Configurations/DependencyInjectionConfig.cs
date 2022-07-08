using NSE.Catalogo.Data;
using NSE.Catalogo.Data.Repository;
using NSE.Catalogo.Models;

namespace NSE.Catalogo.Configurations;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<CatalogoContext>();
    }
}
