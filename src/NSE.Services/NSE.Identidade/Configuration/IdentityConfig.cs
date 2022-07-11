using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSE.Identidade.Data;
using NSE.Identidade.Extensions;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Identidade.Configuration;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddJwksManager().PersistKeysToDatabaseStore<ApplicationDbContext>();

        // ApplicationDbContext Config
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Identity Config
        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentityMensagensPortugues>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // JWT Config
        services.AddJwtConfiguration(configuration);

        return services;
    }
}
