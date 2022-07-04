using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;

namespace MVC.Controllers;

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

    [Route("carrinho")]
    public async Task<IActionResult> Index()
    {
        return View(await _carrinhoService.ObterCarrinho());
    }

    [HttpPost]
    [Route("carrinho/adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
    {
        var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

        ValidarItemCarrinho(produto, itemProduto.Quantidade);

        if (!OperacaoValida()) return View("Index", await _carrinhoService.ObterCarrinho());

        itemProduto.Nome = produto!.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;

        var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

        if (ResponseResultPossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        return RedirectToAction("index");
    }

    [HttpPost]
    [Route("carrinho/atualizar-item")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        ValidarItemCarrinho(produto, quantidade);

        if (!OperacaoValida()) return View("Index", await _carrinhoService.ObterCarrinho());

        var itemProduto = new ItemProdutoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
        var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

        if (ResponseResultPossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/remover-item")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var produto = await _catalogoService.ObterPorId(produtoId);

        if (produto is null)
        {
            AdicionarErroValidacao("Produto inexistente!");
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);

        if (ResponseResultPossuiErros(resposta))
        {
            return View("Index", await _carrinhoService.ObterCarrinho());
        }

        return RedirectToAction("index");
    }

    private void ValidarItemCarrinho(ProdutoViewModel produto, int quantidade)
    {
        if (produto is null)
            AdicionarErroValidacao("Produto inexistente!");

        if (quantidade < 1)
            AdicionarErroValidacao($"Escolha ao menos uma unidade do produto {produto?.Nome}");

        if (quantidade > produto?.QuantidadeEstoque)
            AdicionarErroValidacao($"O produto {produto?.Nome} possui {produto?.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
