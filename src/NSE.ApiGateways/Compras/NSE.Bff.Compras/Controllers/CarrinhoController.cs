using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return CustomResponse();
    }

    [HttpGet("compras/carrinho-quantidade")]
    public async Task<IActionResult> ObterQuantidadeCarrinho()
    {
        return CustomResponse();
    }

    [HttpPost("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho()
    {
        return CustomResponse();
    }

    [HttpPut("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho()
    {
        return CustomResponse();
    }

    [HttpDelete("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho()
    {
        return CustomResponse();
    }
}
