using Core.Comunication;
using Microsoft.Extensions.Options;
using MVC.Extensions;
using MVC.Models;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface IComprasBffService
{
    // Carrinho
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(string voucher);

    // Pedido
    Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
    Task<PedidoViewModel> ObterUltimoPedido();
    Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
    PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel? endereco);
}

public class ComprasBffService : Service, IComprasBffService
{
    private readonly HttpClient _httpClient;

    public ComprasBffService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl
            ?? throw new ArgumentNullException(nameof(settings.Value.ComprasBffUrl)));
    }

    #region Carrinho

    public async Task<CarrinhoViewModel> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho/");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho-quantidade/");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<int>(response);
    }
    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
    {
        var itemContent = ObterConteudo(carrinho);

        var response = await _httpClient.PostAsync("/compras/carrinho/items/", itemContent);

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel item)
    {
        var itemContent = ObterConteudo(item);

        var response = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
    public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
    {
        var itemContent = ObterConteudo(voucher);

        var response = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemContent);

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    #endregion

    #region Pedido

    public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
    {
        var pedidoContent = ObterConteudo(pedidoTransacao);

        var response = await _httpClient.PostAsync("/compras/pedido/", pedidoContent);

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }

    public async Task<PedidoViewModel> ObterUltimoPedido()
    {
        var response = await _httpClient.GetAsync("/compras/pedido/ultimo/");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<PedidoViewModel>(response);
    }

    public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
    {
        var response = await _httpClient.GetAsync("/compras/pedido/lista-cliente/");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<IEnumerable<PedidoViewModel>>(response);
    }

    public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel? endereco)
    {
        var pedido = new PedidoTransacaoViewModel
        {
            ValorTotal = carrinho.ValorTotal,
            Itens = carrinho.Itens,
            Desconto = carrinho.Desconto,
            VoucherUtilizado = carrinho.VoucherUtilizado,
            VoucherCodigo = carrinho.Voucher?.Codigo
        };

        if (endereco is not null)
        {
            pedido.Endereco = new EnderecoViewModel
            {
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cep = endereco.Cep,
                Complemento = endereco.Complemento,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado
            };
        }

        return pedido;
    }

    #endregion
}