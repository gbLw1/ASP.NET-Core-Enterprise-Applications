using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.Models;

namespace NSE.Catalogo.Controllers;

[ApiController]
public class CatalogoController : Controller
{
    private readonly IProdutoRepository _produtoRepository;

    public CatalogoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index()
    {
        return await _produtoRepository.ObterTodos();
    }

    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto> ProdutoDetalhe(Guid id)
    {
        return await _produtoRepository.ObterPorId(id);
    }
}
