using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using NSE.Clientes.Application.Commands;
using NSE.Clientes.Application.Events;
using NSE.Clientes.Data;
using NSE.Clientes.Data.Repository;
using NSE.Clientes.Models;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IAspNetUser, AspNetUser>();

        services.AddScoped<IMediatorHandler, MediatorHandler>();

        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
        services.AddScoped<IRequestHandler<AdicionarEnderecoCommand, ValidationResult>, ClienteCommandHandler>();

        services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientesContext>();
    }
}