using Core.Comunication;
using Microsoft.Extensions.Options;
using MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface IComprasBffService
{
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<int> ObterQuantidadeCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
    Task<ResponseResult> AplicarVoucherCarrinho(string voucher);
}

public class ComprasBffService : Service, IComprasBffService
{
    private readonly HttpClient _httpClient;

    public ComprasBffService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl ?? throw new ArgumentNullException(nameof(settings.Value.ComprasBffUrl)));
    }

    public async Task<CarrinhoViewModel> ObterCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
    }

    public async Task<int> ObterQuantidadeCarrinho()
    {
        var response = await _httpClient.GetAsync("/compras/carrinho-quantidade");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<int>(response);
    }

    public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
    {
        var itemContent = ObterConteudo(itemCarrinho);

        var response = await _httpClient.PostAsync("/compras/carrinho/items", itemContent);

        if (HttpResponseHasErrors(response))
        {
            return await DeserializarObjetoResponse<ResponseResult>(response);
        }

        return RetornoOk();
    }

    public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel itemCarrinho)
    {
        var itemContent = ObterConteudo(itemCarrinho);

        var response = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);

        if (HttpResponseHasErrors(response))
        {
            return await DeserializarObjetoResponse<ResponseResult>(response);
        }

        return RetornoOk();
    }

    public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

        if (HttpResponseHasErrors(response))
        {
            return await DeserializarObjetoResponse<ResponseResult>(response);
        }

        return RetornoOk();
    }

    public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
    {
        var itemContent = ObterConteudo(voucher);

        var response = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemContent);

        if (HttpResponseHasErrors(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

        return RetornoOk();
    }
}
