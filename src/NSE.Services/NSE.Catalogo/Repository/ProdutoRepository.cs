using Core.Data;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Data;
using NSE.Catalogo.Models;

namespace NSE.Catalogo.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly CatalogoContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public ProdutoRepository(CatalogoContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ObterTodos()
        => await _context.Produtos.AsNoTracking().ToListAsync();

    public async Task<Produto> ObterPorId(Guid id)
        => await _context.Produtos.FindAsync(id) ?? throw new ArgumentException("Produto não encontrado.");

    public void Adicionar(Produto produto)
        => _context.Produtos.Add(produto);

    public void Atualizar(Produto produto)
        => _context.Produtos.Update(produto);

    public void Dispose()
        => _context?.Dispose();
}
