using Core.Messages;
using FluentValidation.Results;
using MediatR;
using NSE.Clientes.Models;

namespace NSE.Clientes.Application.Commands;

public class ClienteCommandHandler : CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult>
{
    public async Task<ValidationResult> Handle(
        RegistrarClienteCommand message,
        CancellationToken cancellationToken)
    {
        if (!message.IsValid())
        {
            return message.ValidationResult;
        }

        var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

        // validações de negócio

        // persistir no banco
        if (true) // erro cpf ja cadastrado
        {
            AdicionarErro("Este CPF já está em uso.");
            return ValidationResult;
        }
    }
}
