using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.Controllers;

[Authorize]
public class CatalogoController : MainController
{
    private readonly IProdutoRepository _produtoRepository;

    public CatalogoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [AllowAnonymous]
    [HttpGet("catalogo/produtos")]
    public async Task<PagedResult<Produto>> Index(
        [FromQuery] int pageSize = 8,
        [FromQuery] int pageIndex = 1,
        [FromQuery] string? query = null)
    {
        return await _produtoRepository.ObterTodos(pageSize, pageIndex, query);
    }

    [ClaimsAuthorize("Catalogo", "Ler")]
    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto> ProdutoDetalhe(Guid id)
    {
        return await _produtoRepository.ObterPorId(id);
    }

    [HttpGet("catalogo/produtos/lista/{ids}")]
    public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids)
    {
        return await _produtoRepository.ObterProdutosPorId(ids);
    }
}
