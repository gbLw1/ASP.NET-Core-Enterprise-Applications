using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.Data;
using NSE.Carrinho.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.Controllers;

public class CarrinhoController : MainController
{
    private readonly IAspNetUser _user;
    private readonly CarrinhoContext _context;

    public CarrinhoController(IAspNetUser user, CarrinhoContext context)
    {
        _user = user;
        _context = context;
    }

    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
    }

    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho is null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);

        if (!OperacaoValida())
        {
            return CustomResponse();
        }

        await PersistirDados();

        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho is null) throw new Exception();

        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);

        if (itemCarrinho is null)
        {
            return CustomResponse();
        }

        carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

        _context.CarrinhoItens.Update(itemCarrinho);
        _context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();

        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        var carrinho = await ObterCarrinhoCliente();

        if (carrinho is null) throw new Exception();

        var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);

        if (itemCarrinho is null)
        {
            return CustomResponse();
        }

        carrinho.RemoverItem(itemCarrinho);

        _context.CarrinhoItens.Remove(itemCarrinho);
        _context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();

        return CustomResponse();
    }

    private async Task<CarrinhoCliente?> ObterCarrinhoCliente()
    {
        return await _context.CarrinhoCliente
                     .Include(c => c.Itens)
                     .FirstOrDefaultAsync(c => c.ClienteId == _user.ObterUserId());
    }

    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        var carrinho = new CarrinhoCliente(_user.ObterUserId());
        carrinho.AdicionarItem(item);
        _context.CarrinhoCliente.Add(carrinho);
    }

    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        var produtoExistente = carrinho.CarrinhoItemExistente(item);

        carrinho.AdicionarItem(item);

        if (produtoExistente)
        {
            _context.CarrinhoItens.Update(carrinho.ObterProdutoPorId(item.ProdutoId));
        }
        else
        {
            _context.CarrinhoItens.Add(item);
        }

        _context.CarrinhoCliente.Update(carrinho);
    }

    private async Task<CarrinhoItem?> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente? carrinho, CarrinhoItem? item = null)
    {
        if (item != null && produtoId != item.ProdutoId)
        {
            Erros.Add("O item não corresponde ao informado");
            return null;
        }

        if (carrinho is null)
        {
            Erros.Add("Carrinho não encontrado");
            return null;
        }

        var itemCarrinho = await _context.CarrinhoItens
            .FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

        if (itemCarrinho is null || !carrinho.CarrinhoItemExistente(itemCarrinho))
        {
            Erros.Add("O item não está no carrinho");
            return null;
        }

        return itemCarrinho;
    }

    private async Task PersistirDados()
    {
        var result = await _context.SaveChangesAsync();
        if (result <= 0) Erros.Add("Não foi possível persistir os dados no banco");
    }
}
