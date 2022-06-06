using Core.Mediator;
using FluentValidation.Results;
using MediatR;
using NSE.Clientes.Application.Commands;

namespace NSE.Clientes.Configuration;

public static class DependencyInjectionConfig
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IMediatorHandler, MediatorHandler>();
        services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();
    }
}