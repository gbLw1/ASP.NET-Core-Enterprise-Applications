using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Carrinho.Controllers;

[Authorize]
public class CarrinhoController : MainController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICatalogoService _catalogoService;

    public CarrinhoController(ICarrinhoService carrinhoService,
                              ICatalogoService catalogoService)
    {
        _carrinhoService = carrinhoService;
        _catalogoService = catalogoService;
    }

    [HttpGet("compras/carrinho")]
    public async Task<IActionResult> Index()
    {
        return CustomResponse(await _carrinhoService.ObterCarrinho());
    }

    [HttpGet("compras/carrinho-quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        var quantidade = await _carrinhoService.ObterCarrinho();
        return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
    }

    [HttpPost("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade);

        if (!OperacaoValida()) return CustomResponse();

        itemProduto.Nome = produto.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;

        var response = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

        return CustomResponse(response);
    }

    [HttpPut("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade);

        if (!OperacaoValida()) return CustomResponse();

        var response = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

        return CustomResponse(response);
    }

    [HttpDelete("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        if (produto is null)
        {
            AdicionarErroProcessamento("Produto inexistente!");
            return CustomResponse();
        }

        var response = await _carrinhoService.RemoverItemCarrinho(produtoId);

        return CustomResponse(response);
    }

    private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade)
    {
        if (produto is null) AdicionarErroProcessamento("Produto inexistente!");
        if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto?.Nome}");

        var carrinho = await _carrinhoService.ObterCarrinho();
        var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto?.Id);

        if (itemCarrinho is not null && itemCarrinho.Quantidade + quantidade > produto?.QuantidadeEstoque)
        {
            AdicionarErroProcessamento($"O produto {produto?.Nome} possui {produto?.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
            return;
        }

        if (quantidade > produto?.QuantidadeEstoque) AdicionarErroProcessamento($"O produto {produto?.Nome} possui {produto?.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
