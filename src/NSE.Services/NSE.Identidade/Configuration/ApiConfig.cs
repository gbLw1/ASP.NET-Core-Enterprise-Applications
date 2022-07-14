using NSE.Identidade.API.Services;
using NSE.WebAPI.Core.Identidade;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Identidade.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<IAspNetUser, AspNetUser>();
        services.AddScoped<AuthenticationService>();

        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCors(options =>
        {
            options.AllowAnyHeader();
            options.AllowAnyMethod();
            options.AllowAnyOrigin();
        });

        app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthConfiguration();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // localhost/jwks
        app.UseJwksDiscovery();

        return app;
    }
}
