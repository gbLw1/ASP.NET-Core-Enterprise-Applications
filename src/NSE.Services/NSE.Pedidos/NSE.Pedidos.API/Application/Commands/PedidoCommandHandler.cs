using Core.Messages;
using Core.Messages.Integration;
using FluentValidation.Results;
using MediatR;
using NSE.MessageBus;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.API.Application.Events;
using NSE.Pedidos.Domain.Pedidos;
using NSE.Pedidos.Domain.Vouchers;
using NSE.Pedidos.Domain.Vouchers.Specs;

namespace NSE.Pedidos.API.Application.Commands;

public class PedidoCommandHandler : CommandHandler,
    IRequestHandler<AdicionarPedidoCommand, ValidationResult>
{
    private readonly IVoucherRepository _voucherRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IMessageBus _bus;

    public PedidoCommandHandler(
        IVoucherRepository voucherRepository,
        IPedidoRepository pedidoRepository,
        IMessageBus bus)
    {
        _voucherRepository = voucherRepository;
        _pedidoRepository = pedidoRepository;
        _bus = bus;
    }

    public async Task<ValidationResult> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        // Validação do comando
        if (message.Valido() is false) return message.ValidationResult;

        // Mapear pedido
        var pedido = MapearPedido(message);

        // Aplicar Voucher se houver
        if (await AplicarVoucher(message, pedido) is false) return ValidationResult;

        // Validar pedido
        if (ValidarPedido(pedido) is false) return ValidationResult;

        // Processar pagamento
        if (await ProcessarPagamento(pedido, message) is false) return ValidationResult;

        // Se pagamento tudo ok
        pedido.AutorizarPedido();

        // Adicionar evento
        pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

        // Adicionar Pedido Repositorio
        _pedidoRepository.Adicionar(pedido);

        // Persistir dados de pedido e voucher
        return await PersistirDados(_pedidoRepository.UnitOfWork);
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado) return true;

        var voucher = await _voucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo!);
        if (voucher is null)
        {
            AdicionarErro("O vVoucher informado não existe!");
            return false;
        }

        var voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(erro => AdicionarErro(erro.ErrorMessage));
            return false;
        }

        pedido.AtribuirVoucher(voucher);
        voucher.DebitarQuantidade();

        _voucherRepository.Atualizar(voucher);

        return true;
    }

    private Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        var endereco = new Endereco
        {
            Logradouro = message.Endereco!.Logradouro,
            Numero = message.Endereco.Numero,
            Complemento = message.Endereco.Complemento,
            Bairro = message.Endereco.Bairro,
            Cidade = message.Endereco.Cidade,
            Estado = message.Endereco.Estado,
            Cep = message.Endereco.Cep
        };

        var pedido = new Pedido(message.ClienteId, message.ValorTotal, message.PedidoItems!.Select(PedidoItemDTO.ParaPedidoItem).ToList(), message.VoucherUtilizado, message.Desconto);

        pedido.AtribuirEndereco(endereco);
        return pedido;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        var pedidoValorOriginal = pedido.ValorTotal;
        var pedidoDesconto = pedido.Desconto;

        pedido.CalcularValorPedido();

        if (pedido.ValorTotal != pedidoValorOriginal)
        {
            AdicionarErro("O valor total do pedido não confere com o cálculo do pedido");
            return false;
        }

        if (pedido.Desconto != pedidoDesconto)
        {
            AdicionarErro("O valor total não confere com o cálculo do pedido");
            return false;
        }

        return true;
    }

    public async Task<bool> ProcessarPagamento(Pedido pedido, AdicionarPedidoCommand message)
    {
        var pedidoIniciado = new PedidoIniciadoIntegrationEvent
        {
            PedidoId = pedido.Id,
            ClienteId = pedido.ClienteId,
            Valor = pedido.ValorTotal,
            TipoPagamento = 1, // Credito
            NomeCartao = message.NomeCartao,
            NumeroCartao = message.NumeroCartao,
            MesAnoVencimento = message.ExpiracaoCartao,
            CVV = message.CvvCartao
        };

        var result = await _bus.RequestAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(pedidoIniciado);

        if (result.ValidationResult.IsValid) return true;

        foreach (var erro in result.ValidationResult.Errors)
        {
            AdicionarErro(erro.ErrorMessage);
        }

        return false;
    }
}
