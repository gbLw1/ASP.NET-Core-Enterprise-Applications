using Core.Messages;
using FluentValidation.Results;
using MediatR;
using NSE.Clientes.Application.Events;
using NSE.Clientes.Models;

namespace NSE.Clientes.Application.Commands;

public class ClienteCommandHandler : CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult>
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ValidationResult> Handle(
        RegistrarClienteCommand message,
        CancellationToken cancellationToken)
    {
        if (message.Valido() is false)
        {
            return message.ValidationResult;
        }

        var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

        var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf!.Numero!);

        // Cliente já cadastrado
        if (clienteExistente is not null)
        {
            AdicionarErro("Este CPF já está em uso.");
            return ValidationResult;
        }

        _clienteRepository.Adicionar(cliente);

        cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));

        return await PersistirDados(_clienteRepository.UnitOfWork);
    }
}
