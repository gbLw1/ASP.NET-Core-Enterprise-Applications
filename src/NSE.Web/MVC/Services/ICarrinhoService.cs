using Core.Comunication;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface ICarrinhoService
{
    Task<CarrinhoViewModel> ObterCarrinho();
    Task<ResponseResult> AdicionarItemCarrinho(ItemProdutoViewModel produto);
    Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);
    Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
}
