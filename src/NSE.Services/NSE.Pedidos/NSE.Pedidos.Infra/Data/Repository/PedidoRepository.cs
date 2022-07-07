using Core.Data;
using Microsoft.EntityFrameworkCore;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.Infra.Data.Repository;

public class PedidoRepository : IPedidoRepository
{
    private readonly PedidosContext _context;

    public PedidoRepository(PedidosContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> ObterPorId(Guid id)
    {
        return await _context.Pedidos.FindAsync(id);
    }

    public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
    {
        return await _context.Pedidos
            .Include(p => p.PedidoItems)
            .AsNoTrackingWithIdentityResolution()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();
    }

    public void Adicionar(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
    }

    public void Atualizar(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
    }

    public async Task<PedidoItem?> ObterItemPorId(Guid id)
    {
        return await _context.PedidoItems.FindAsync(id);
    }

    public async Task<PedidoItem?> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
    {
        return await _context.PedidoItems
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId && p.ProdutoId == produtoId);
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Dispose()
    {
        _context.Dispose();
    }
}