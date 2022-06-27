using Microsoft.AspNetCore.Mvc;
using NSE.Carrinho.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.Controllers;

public class CarrinhoController : MainController
{
    private readonly IAspNetUser _user;

    public CarrinhoController(IAspNetUser user)
    {
        _user = user;
    }

    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return null;
    }

    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }
}
