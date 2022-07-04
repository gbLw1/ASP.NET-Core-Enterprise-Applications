using NSE.WebApp.MVC.Models;
using Refit;

namespace MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

// Refit
public interface ICatalogoServiceRefit
{
    [Get("/catalogo/produtos")]
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();

    [Get("/catalogo/produtos/{id}")]
    Task<ProdutoViewModel> ObterPorId(Guid id);
}