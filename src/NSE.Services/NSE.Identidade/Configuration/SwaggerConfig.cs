using Microsoft.OpenApi.Models;

namespace NSE.Identidade.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NerdStore Enterprise Identidade API",
                Description = "Esta API faz parte do curso ASP.NET Core Enterprise Applications.",
                Contact = new OpenApiContact { Name = "Gabriel - OnFriday", Email = "gabriel.grassi@onfriday.com.br" },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

        return app;
    }
}
