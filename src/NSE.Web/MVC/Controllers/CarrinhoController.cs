using Microsoft.AspNetCore.Mvc;
using MVC.Services;
using NSE.WebApp.MVC.Models;

namespace MVC.Controllers;

public class CarrinhoController : MainController
{
    private readonly IComprasBffService _comprasBffService;

    public CarrinhoController(IComprasBffService comprasBffService)
    {
        _comprasBffService = comprasBffService;
    }

    [Route("carrinho")]
    public async Task<IActionResult> Index()
    {
        return View(await _comprasBffService.ObterCarrinho());
    }

    [HttpPost]
    [Route("carrinho/adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
    {
        var response = await _comprasBffService.AdicionarItemCarrinho(itemCarrinho);

        if (ResponseResultPossuiErros(response))
            return View("Index", await _comprasBffService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/atualizar-item")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
    {
        var itemCarrinho = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };

        var response = await _comprasBffService.AtualizarItemCarrinho(produtoId, itemCarrinho);

        if (ResponseResultPossuiErros(response))
            return View("Index", await _comprasBffService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/remover-item")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var response = await _comprasBffService.RemoverItemCarrinho(produtoId);

        if (ResponseResultPossuiErros(response))
            return View("Index", await _comprasBffService.ObterCarrinho());

        return RedirectToAction("Index");
    }
}
